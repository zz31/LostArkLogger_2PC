using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace LostArkLogger.Utilities
{
    public class CharacterSearch
    {
        public bool captureMode = false;
        public Dictionary<string, characterSearchResult> playerDatas = new Dictionary<string, characterSearchResult>();//username, mgxData
        public string[] latestUserList = { null, null, null, null, null, null, null, null };
        public int latestUserPointer = 0;// % 8
        public bool specCheckerEnabled = false;
        private static readonly object addLock = new object();
        private static readonly object parseLock = new object();

        private string userNameStr(string uname)
        {
            if (captureMode == true)
            {
                return "[ID숨김]";
            } else
            {
                return uname;
            }
        }
        public characterSearchResult getPlayerInfo(string uname)
        {
            if (uname == null) return null;
            if (playerDatas.ContainsKey(uname))
            {
                return playerDatas[uname];
            }
            else
            {
                return null;
            }
        }
        public characterSearchResult[] getPlayerLast8()
        {
            if (playerDatas.Count == 0) return null;
            List<characterSearchResult> retArr = new List<characterSearchResult>();

            for (int k = 0; k < 8; k++)
            {
                characterSearchResult tmpStr = getPlayerInfo(latestUserList[k]);
                if (tmpStr != null) retArr.Add(tmpStr);
            }

            if (retArr.Count == 0) return null;
            return retArr.ToArray();
        }

        public Action onDataUpdated;//->overlay, invalidate
        public Func<Overlay.Level> getLvl;
        public bool parsing = false;

        public void doParse()
        {
            if (parsing == false && getLvl() == Overlay.Level.Damage && specCheckerEnabled == true)//대미지일때만 체크해서 맨아래쪽에 overlay에서 출력함
            {
                parsing = true;
                Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    bool continueFlag = false;
                    try
                    {//async parse, last 8
                        for (int i = 0; i < latestUserList.Length; i++)
                        {
                            if (latestUserList[i] == null) continue;
                            if (getPlayerInfo(latestUserList[i]) == null)
                            {
                                characterSearchResult tmp_str = await checkMgx(latestUserList[i]);
                                string inv_str = await checkInven(latestUserList[i]);
                                lock (parseLock)
                                {
                                    if (tmp_str != null && latestUserList[i] != null)
                                    {
                                        if (inv_str != null && inv_str != "0") tmp_str.resultInven = "박제" +inv_str+ "회";
                                        playerDatas[latestUserList[i]] = tmp_str;
                                        onDataUpdated?.Invoke();//overlay 새로 갱신
                                    }
                                }
                                continueFlag = true;//한개라도 수정되었으면 다시한번 체크, 8개다 continue시 contflag = false로 파싱이끝난다
                                break;//한개만 업뎃하고 다시 맨앞부터간다
                            }
                        }
                    }
                    catch (Exception e) { }
                    parsing = false;
                    if (continueFlag == true && specCheckerEnabled == true) doParse();
                });
            }
        }
        public void onNewPC(string username, ushort classid)
        {
            if ((username == "You" || classid == 0 || username.Length < 2 || username.Length > 12)) return;
            lock (addLock)
            {
                if (!latestUserList.Contains(username))
                {
                    latestUserList[latestUserPointer] = username;
                    latestUserPointer++;
                    latestUserPointer %= 8;
                    Console.WriteLine("Add : " + username);
                }
                doParse();
            }
        }
        public class PlayerData
        {
            public string userName, mgxData_parsed, invenData_parsed;

            public PlayerData(string u)
            {
                userName = u;
            }
        }
        public class characterSearchResult
        {
            public string username, resultTitle, resultWarn, resultContent, resultInven;
            public characterSearchResult(string u, string rT, string rW, string rC)
            {
                this.username = u;
                this.resultTitle = rT;
                this.resultWarn = rW;
                this.resultContent = rC;
                this.resultInven = null;
            }
            public characterSearchResult(string u, string rT, string rW, string rC, string rI)
            {
                this.username = u;
                this.resultTitle = rT;
                this.resultWarn = rW;
                this.resultContent = rC;
                this.resultInven = rI;
            }
        }
        public async Task<characterSearchResult> checkMgx(string username)
        {
            if (username == null || username.Length > 12 || username.Trim().Length == 0) { return null; }
            Console.WriteLine("PARSEmgx >> " + username);
            return await Task.Run(() =>
            {
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create("https://www.mgx.kr/lostark/character/?character_name=" + Uri.EscapeUriString(username));
                    request.Method = "GET";
                    request.Accept = "application/json, text/plain, */*";
                    request.Referer = "https://www.mgx.kr/lostark/";
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.0.0 Safari/537.36";
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                    request.Headers.Add("Accept-Language", "ko-KR,ko;q=0.9,en-US;q=0.8,en;q=0.7");
                    request.Headers.Add("x-requested-with", "XMLHttpRequest");
                    request.AutomaticDecompression = DecompressionMethods.GZip;
                    var web = new HtmlAgilityPack.HtmlDocument();
                    web.Load(request.GetResponse().GetResponseStream(), Encoding.UTF8);
                    string chName = web.DocumentNode.SelectNodes("//div[@class='character_name']")[0].InnerText.Split(' ')[2].ToString();
                    string chLevel = web.DocumentNode.SelectNodes("//div[@class='reached_item_level']")[0].InnerText.Split(' ')[1].ToString();
                    string expLevel = web.DocumentNode.SelectNodes("//div[@class='expedition_level']")[0].InnerText.Split('.')[1].ToString();
                    var cardEffectList = web.DocumentNode.SelectNodes("//div[@class='card_option']/div[@class='effect_box']/div[@class='effect']");
                    var carvingList = web.DocumentNode.SelectNodes("//div[@class='carving']");
                    var itemList = web.DocumentNode.SelectNodes("//div[@class='equipment_box']/div[@class='equipment equipment_left']");//weap, etc

                    //닉넴, 레벨, 원대
                    string isWarnMsg = "";//[WARN]
                    string respTxt = userNameStr(chName) + " (" + chLevel + "Lv 원대" + expLevel + ") : ";
                    if (int.Parse(expLevel) < 120) isWarnMsg += "원대";

                    //각인
                    string rkr_3 = "";
                    string rkr_other = "";
                    string rkr_debuff = "";
                    string[] supSkill = { "축복의 오라", "만개", "절실한 구원" };
                    string[] mutantSkill = { "멈출 수 없는 충동", "진화의 유산" };
                    bool isSupport = false;
                    bool isMutant = false;
                    if (carvingList != null)
                    {
                        for (int i = 0; i < carvingList.Count; i++)
                        {
                            string cur_c = web.DocumentNode.SelectNodes("//div[@class='carving']/div[@class='carving_category']")[i].InnerText.Replace("\n", "").Trim();
                            string cur_l = web.DocumentNode.SelectNodes("//div[@class='carving']/div[@class='carving_level']")[i].InnerText.Replace("\n", "").Trim();
                            if (supSkill.Contains(cur_c) == true && cur_l == "Lv.3") isSupport = true;
                            if (mutantSkill.Contains(cur_c) == true) isMutant = true;
                            if (!cur_c.EndsWith("감소"))
                            {
                                switch (cur_l)
                                {
                                    case "Lv.3":
                                        rkr_3 += cur_c.Substring(0, 1);
                                        break;
                                    case "Lv.2":
                                        rkr_other += cur_c.Substring(0, 1) + "2";
                                        break;
                                    case "Lv.1":
                                        rkr_other += cur_c.Substring(0, 1) + "1";
                                        break;
                                }
                            }
                            else
                            {
                                string down_str = "";
                                switch (cur_c)
                                {
                                    case "공격력 감소":
                                        down_str = "공감";
                                        break;
                                    case "공격속도 감소":
                                        down_str = "공속감";
                                        break;
                                    case "이동속도 감소":
                                        down_str = "이감";
                                        break;
                                    case "방어력 감소":
                                        down_str = "방감";
                                        break;
                                }
                                switch (cur_l)
                                {
                                    case "Lv.3":
                                        rkr_debuff += down_str + "3";
                                        break;
                                    case "Lv.2":
                                        rkr_debuff += down_str + "2";
                                        break;
                                    case "Lv.1":
                                        rkr_debuff += down_str + "1";
                                        break;
                                }
                            }
                        }
                    } else { rkr_3 = "각인없음"; }
                    if (rkr_3.Length < 4) isWarnMsg += "각인";

                    //카드셋
                    if (cardEffectList != null)
                    {
                        for (int i = 0; i < cardEffectList.Count; i++)
                        {
                            switch (web.DocumentNode.SelectNodes("//div[@class='card_option']/div[@class='effect_box']/div[@class='effect']/div[@class='effect_name']")[i].InnerText.Trim())
                            {
                                case "카제로스의 군단장 6세트 (18각성합계)":
                                    if (!respTxt.EndsWith("암구30"))
                                    {
                                        respTxt += "암구18";
                                    }
                                    break;
                                case "카제로스의 군단장 6세트 (30각성합계)":
                                    if (respTxt.EndsWith("암구18"))
                                    {
                                        respTxt = respTxt.Replace("암구18", "암구30");
                                    }
                                    else
                                    {
                                        respTxt += "암구30";
                                    }
                                    break;
                                case "라제니스의 운명 2세트 (10각성합계)"://세우라제
                                    if (respTxt.EndsWith("세우"))
                                    {
                                        respTxt = respTxt.Replace("세우", "세우라제");
                                    }
                                    else
                                    {
                                        respTxt += "라제";
                                    }
                                    break;
                                case "세 우마르가 오리라 3세트 (15각성합계)"://세우라제
                                    if (respTxt.EndsWith("라제"))
                                    {
                                        respTxt = respTxt.Replace("라제", "세우라제");
                                    }
                                    else
                                    {
                                        respTxt += "세우";
                                    }
                                    break;
                                case "남겨진 바람의 절벽 6세트 (30각성합계)":
                                    if (respTxt.EndsWith("남바12"))
                                    {
                                        respTxt = respTxt.Replace("남바12", "남바30");
                                    }
                                    else
                                    {
                                        respTxt += "남바30";
                                    }
                                    break;
                                case "남겨진 바람의 절벽 6세트 (12각성합계)":
                                    if (!respTxt.EndsWith("남바30")) respTxt += "남바12";
                                    break;
                                case "세상을 구하는 빛 6세트 (30각성합계)":
                                    if (respTxt.EndsWith("세구18"))
                                    {
                                        respTxt = respTxt.Replace("세구18", "세구30");
                                    }
                                    else
                                    {
                                        respTxt += "세구30";
                                    }
                                    break;
                                case "세상을 구하는 빛 6세트 (18각성합계)":
                                    if (!respTxt.EndsWith("세구30")) respTxt += "세구18";
                                    break;
                            }
                        }
                    } else { respTxt += "카드없음"; }
                    if (!respTxt.Contains("세구30") && !respTxt.Contains("세구18") && !isSupport &&
                    !respTxt.Contains("세우라제") && !respTxt.Contains("암구18") && !respTxt.Contains("암구30")) isWarnMsg += "카드";


                    //무기, 유물
                    string wInfo = "";
                    string setItem = "";
                    int trashdetect_reliccnt = 0;
                    string[] setname = { "지배", "배신", "갈망", "파괴", "매혹", "사멸", "악몽", "환각", "구원" };
                    if (itemList != null)
                    {
                        Dictionary<string, int> setInfo = new Dictionary<string, int>();
                        for (int i = 0; i < itemList.Count; i++)
                        {
                            string itype = web.DocumentNode.SelectNodes("//div[@class='equipment_box']/div[@class='equipment equipment_left']/div[@class='equipment_info']/div[@class='equipment_part_name']/div[@class='part_name']")[i].InnerText.Trim();
                            string iname = web.DocumentNode.SelectNodes("//div[@class='equipment_box']/div[@class='equipment equipment_left']/div[@class='equipment_info']/div[@class='equipment_name']")[i].InnerText.Trim();
                            if (itype == "무기")
                            {
                                wInfo = iname.Split(' ')[0].Replace("\n", " ").Trim() + " ";//+xx, 구 전설템은 오류있음
                            }
                            for (int j = 0; j < setname.Length; j++)
                            {
                                if (iname.Contains(setname[j]))
                                {
                                    if (setInfo.ContainsKey(setname[j]))
                                    {
                                        setInfo[setname[j]]++;
                                    }
                                    else
                                    {
                                        setInfo[setname[j]] = 1;
                                    }
                                }
                            }
                        }
                        var keys = setInfo.Keys.ToArray();
                        int relic_cnt = 0;
                        for (int i = 0; i < setInfo.Count; i++)
                        {
                            setItem += keys[i] + setInfo[keys[i]].ToString();
                            trashdetect_reliccnt += setInfo[keys[i]] % 2;//유물셋이 짝수 가아니면 경고
                            relic_cnt += setInfo[keys[i]];//총합 6세트가 아닌경우 reliccnt를 1 로 바꿔서 경고표시
                        }
                        if (relic_cnt != 6) trashdetect_reliccnt = 1;
                    }
                    if (wInfo.Length == 0) wInfo = "무기X";
                    if (setItem.Length == 0) setItem = "유물X";
                    if (trashdetect_reliccnt != 0) isWarnMsg += "세트";

                    //보석
                    Dictionary<string, int> jewelCnt = new Dictionary<string, int>();
                    string jewelStr = "";
                    int upperLv5 = 0;
                    var jewels = web.DocumentNode.SelectNodes("//div[@class='jewlry_box']/div[@class='jewlry']/div[@class='jewlry_img']/div[@class='jewlry_level']");
                    if (jewels != null)
                    {
                        for (int i = 0; i < jewels.Count; i++)
                        {
                            if (jewelCnt.ContainsKey(jewels[i].InnerText))
                            {
                                jewelCnt[jewels[i].InnerText]++;
                            }
                            else
                            {
                                jewelCnt[jewels[i].InnerText] = 1;
                            }
                        }
                        jewelStr = "보석 { ";
                        var jDictKey = jewelCnt.Keys.ToArray();
                        for (int i = 0; i < jDictKey.Length; i++)
                        {
                            jewelStr += jDictKey[i] + "Lv[" + jewelCnt[jDictKey[i]].ToString() + "] ";
                            switch (jDictKey[i])
                            {
                                case "1":
                                case "2":
                                case "3":
                                case "4":
                                    break;
                                default:
                                    upperLv5 += jewelCnt[jDictKey[i]];
                                    break;
                            }
                        }
                        jewelStr += "}";
                    } else
                    {
                        jewelStr = "보석없음";
                    }//오류 : 이벤트 5렙 보석은 표기가안됨
                    if ((isSupport && upperLv5 < 8) || (!isSupport && !isMutant && upperLv5 < 10) || (isMutant && upperLv5 < 2)) isWarnMsg += "보석";

                    //트라이포드(스킬)
                    int[] tripodLev = { 0, 0 };//4lv, 5lv
                    var tripods = web.DocumentNode.SelectNodes("//div[@class='tripod_level']");//("//div[@class='skill_wrapper']/div[@class='skill_block']");
                    if (tripods != null)
                    {
                        for (int i = 0; i < tripods.Count; i++)
                        {
                            switch (tripods[i].InnerText)
                            {
                                case "Lv.4":
                                    tripodLev[0]++;
                                    break;
                                case "Lv.5":
                                    tripodLev[1]++;
                                    break;
                            }
                        }
                    }

                    string tripodStr = "트포 { Lv4[" + tripodLev[0].ToString() + "] Lv5[" + tripodLev[1].ToString() + "] }";
                    double tripodScore = tripodLev[0] * 0.34 + tripodLev[1];
                    if ((isMutant && tripodScore < 2.5) || (isSupport && tripodScore < 3.3) || (!isMutant && !isSupport && tripodScore < 6)) isWarnMsg += "트포";

                    if (isWarnMsg.Length != 0) isWarnMsg = "[경고]" + isWarnMsg;

                    characterSearchResult ret_info = new characterSearchResult(username, respTxt, isWarnMsg, wInfo + " " + setItem + " | " + rkr_3 + rkr_other + rkr_debuff + " | " + jewelStr + " | " + tripodStr);
                    return ret_info;
                    //레벨원대 | 경고 | 닉 무기 카드 각인 21각인 디버프 보석 트포
                }
                catch (Exception e) {
                    //MessageBox.Show(e.StackTrace);
                    return null;
                }
            });
        }

        public void resetLatestUser()
        {
            lock (parseLock)
            {
                latestUserList = new string[] { null, null, null, null, null, null, null, null };
                latestUserPointer = 0;// % 8
                onDataUpdated();
            }
        }

        public async Task<string> checkInven(string username)
        {
            if (username == null || username.Length > 12 || username.Trim().Length == 0) { return null; }
            Console.WriteLine("PARSEinven >> " + username);
            return await Task.Run(() =>
            {
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create("https://www.inven.co.kr/board/lostark/5355?query=list&p=1&sterm=&name=subject&keyword=" + Uri.EscapeUriString(username));
                    request.Method = "GET";
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                    request.Referer = "https://www.inven.co.kr/board/lostark/5355";
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.0.0 Safari/537.36";
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                    request.Headers.Add("Accept-Language", "ko-KR,ko;q=0.9,en-US;q=0.8,en;q=0.7");
                    request.AutomaticDecompression = DecompressionMethods.GZip;
                    var web = new HtmlAgilityPack.HtmlDocument();
                    web.Load(request.GetResponse().GetResponseStream(), Encoding.UTF8);
                    var xNodes = web.DocumentNode.SelectNodes("//*[@id='new-board']/form/div/table/tbody/tr[not(@class='notice all')]");
                    var cnt = xNodes.Count;
                    if (xNodes.Count == 1)
                    {
                        if (xNodes[0].SelectNodes("//*[@class='no-result']")?.Count == 1)
                        {
                            cnt = 0;
                        }
                    }
                    return cnt.ToString();
                } catch(Exception e) { return null; }
            });
        }
    }
}

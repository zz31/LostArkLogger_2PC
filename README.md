# 2PC DPS meter
[The following programs must be installed on the VM or 2nd PC to work]

https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48
-> dps meter is written by c# so it needs .net framework

https://npcap.com/#download or https://www.winpcap.org/
-> if you using my fork it needs npcap or winpcap. (win10 or 11 = npcap, 7 or older = winpcap. but perhaps npcap works at win7)

https://docs.microsoft.com/en-US/cpp/windows/latest-supported-vc-redist?view=msvc-170
-> oo2net_9_win64.dll is written by c++ so needs it

# Todo (order and priority are irrelevant)
1. Resolve misdiagnosis issues
2. Make pull request
2-1. remove loa-details patcher
2-2. temp remove show hp function
3. Add other port support(not 6040)
4. Optimize the codes I made like garbage to normal code
5. Add auto nic/region detect
6. Add auto updater
7. Add custom opcode setting
8. Add custom packet structure parse setting
9. Add 1-PC support(machina)
10. dotnet7
13. 1-PC oo2net support

# FAQ
Q. The file is labeled as a virus in Windows Defender

A. It seems that some codes have been incorrectly diagnosed as viruses. If you're anxious, you can install the vscode and build the source code yourself. I always open all of source code that I upload to the release.

Q. How do I use it in VM/VPN?

A. I don't use VM/VPN, so I don't know how to set it up in detail. If you ask other users on discord, someone will answer.

Q. Is it completely safe to run dps meter with the second pc?

A. No, there is no completely secure way to do so unless you are using Network TAP Device. However, the port mirroring (or packet mirroring) function of the router or switch can almost eliminate the risk.
It's just my personal opinion : even if AGS/Smilegate detect that someone using Packet/Port mirroring, AGS/Smilegate can't detect whether you're using the dps meter on the 2nd PC, so I think the risk of ban is very low.

Q. "I want to do something with your modified code, or there are parts I don't like with your modified code"

A. I have no intention of claiming ownership or copyright of the code I wrote, and I am not interested in how anyone uses it. Don't mind me and just follow the Licenses of the original github(shalzuth/lostarklogger) if you want to do something with the code I modified.

A2. This answer means includes that i don't matter if you're modify the code of my fork as you wish and then make a pull request in the main repository.
I'm not interested in how the code I modified is modified or used. This fork was created to help those who do not know how to modify it themselves.

Q. How do I use this program on my 2nd pc? How does the first pc communicate with the second pc?

A. VM(Virtual 2nd pc), VPN, ICS(Windows Basic Feature), Network Tap Device, port/packet mirroring feature on router/switch

Q. 한국섭은 왜안됨? 한국섭 지원좀 해줘

A. 전 허접이라 themida 언팩못함. 그래서 제가 하려면 실행중인 클라이언트 덤프떠서 분석해야되는데 하다 본계정 정지먹을까봐 안함

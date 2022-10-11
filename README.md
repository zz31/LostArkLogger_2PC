# 2PC DPS meter

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

A. It seems that some codes have been incorrectly diagnosed as viruses. If you're anxious, you can install the vscode yourself and build the source code yourself. I always open all of source code that I upload to the release.

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

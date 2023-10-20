Flames is a fully featured and customizable **ClassiCube Server Software** based on [MCGalaxy 1.9.4.9](https://github.com/UnknownShadow200/MCGalaxy/tree/0a6b0da40b7986d31e52179ef915597fc815e2ee).

**Setup**
-----------------
Download the latest Flames release [from here](https://github.com/DarkBurningFlame/Fire/tree/Flame/Uploads)
* Windows: You will need to install Net Framework 4.8 (Most already have this built-in)
* Linux/macOS: You need to install the [Mono framework](https://www.mono-project.com).

Run **Flames.exe** for a graphical interface, or run **FlamesCLI.exe** for command line only.

Joining your server
-----------------

If you are signed in to classicube.net, you can copy the URL sent using /URL straight into your web browser and start playing.

#### Joining from the ClassiCube client
Click **Direct connect** at the main menu.
![opt1](https://user-images.githubusercontent.com/6509348/60258725-0e05bd00-9919-11e9-8f8c-fbbdc52f04f9.png)

Type your username into *Username*, ```127.0.0.1:25565``` into *IP:Port*, and leave *Mppass* blank. Then click **Connect**.
![opt2](https://user-images.githubusercontent.com/6509348/60258727-0e05bd00-9919-11e9-890d-5c25cdf385c1.png)

#### Make yourself the owner
After joining, you will want to rank yourself as the owner so you can use all commands.

Type ```/rank [your account] Random``` into the bottom text box, then press Enter.



Letting others join your server
-----------------
### LAN players
You need to find out your LAN/local IP address.
*  Windows: Type ```ipconfig``` into **Command Prompt**. Look for ```IPv4 address``` in the output
*  macOS: Type ```ipconfig getifaddr en0``` or ```ipconfig getifaddr en1``` into **Terminal**
*  Linux: Type ```hostname -I``` into **Terminal**. Lan IP is usually the first address in the output

#### Joining from a web browser
Enter the server URL followed by ```?ip=[lan ip]``` into the web browser.<br>
(e.g. http://www.classicube.net/server/play/d1362e7fee1a54365514712d007c8799?ip=192.168.1.30)

#### Joining from the ClassiCube client
* Click **Direct connect** at the main menu
* Type your username into *Username* textbox
* Type ```[lan ip]:25565``` into *IP:Port* textbox (e.g. ```192.168.1.30:25565```)
* Click **Connect**

### Across the internet
You usually need to port forward in your router before other players can join.

#### Joining from a web browser
Enter the server URL into the web browser

#### Joining from the ClassiCube client
* Click **Sign in**
* Type/paste the hash (e.g. ```d1362e7fee1a54365514712d007c8799```) into the *classicube.net/server/play* text box
* Click **Connect**


### Show on classicube.net server list
Click **Settings** in the Flames window, then tick the **Public** checkbox. Then click **Save**.

This makes your server appear in the server list on classicube.net and in the ClassiCube client.

Compiling
-----------------
**With an IDE:**
* Visual Studio: Open `Flames.sln`, click `Build` in the Menubar, then click `Build Solution`. (Or press F6)
* SharpDevelop: Open `Flames.sln`, click `Build` in the menu, then click `Build Solution`. (Or press F8)

**Command line:**
* For Windows: Run `MSBuild command prompt for VS`, then type `msbuild Flames.sln` into the command prompt
* Modern mono: Type `msbuild Flames.sln` into Terminal
* Older mono: Type `xbuild Flames.sln` into Terminal

Copyright/License
-----------------
See LICENSE for Flames license, and license.txt for code used from other software.

Docker support(MCGalaxy)
-----------------
Some **unofficial** dockerfiles for running MCGalaxy in Docker:
* [using Mono](https://github.com/UnknownShadow200/MCGalaxy/pull/577/files)
* [using .NET core](https://github.com/UnknownShadow200/MCGalaxy/pull/629/files)

Documentation(MCGalaxy)
-----------------
* [General documentation](https://github.com/UnknownShadow200/MCGalaxy/wiki)
* [API documentation](https://github.com/ClassiCube/MCGalaxy-API-Documentation)
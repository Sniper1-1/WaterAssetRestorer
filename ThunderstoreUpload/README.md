
# WaterAssetRestorer
Moon and interior devs are likely familiar with how poorly the water assets rip when trying to design their levels. I made this tool to attempt to restore some at runtime. It currenlty can restore the Cave water, the Pool water, and the Gordion/Flooded water. Unfortunately due to the water on Vow, March, and Adamance only existing within those moon scenes, I could not access it at the start of the game to save to be restored later. For that water, I would recommend something like the one dopadream made, which is linked below. ([My thread in the LC Modding Discord](https://discord.com/channels/1168655651455639582/1245084720614604873))

This mod works by searching through a moon after it is loaded to replace the following material names, so you can just copy the one in the Unity editor. It'll be broken in your project but assuming the name remains the same, it'll be replaced in-game.
| Mat name    | description |
| -------- | ------- |
| Water_mat_04  | The water on Gordion/flooded weather    |
| PoolWater | The water in the mansion pool tile     |
| CaveWater    | The water in the cave water tile    |

---


### Before 

![Without](https://imgur.com/Z06aUeJ.png)


### After 

![with](https://imgur.com/FLS6tVR.png)

---

[Dopadream's water](https://discord.com/channels/1168655651455639582/1308174824505479290/1330262292415123466)
![Dopadream's water package](https://media.discordapp.net/attachments/1308174824505479290/1330262292130037770/image.png?ex=688fbccd&is=688e6b4d&hm=4a7c45bf499a448d1946add301f1324d81aa5be61ec754092e57da06951b7a1e&=&format=webp&quality=lossless&width=1555&height=769)



## Credits

- The developers of this mod's dependencies as it literally could not exist without them.
- [Audio Knight](https://www.youtube.com/@knightofaudio) on YouTube for a handy starting tutorial.
- [Nomnomab's project patcher](https://github.com/nomnomab/lc-project-patcher) to access vanilla LC through Unity.
- Debugging tools like [Imperium](https://thunderstore.io/c/lethal-company/p/giosuel/Imperium/) and [UnityExplorer](https://thunderstore.io/c/lethal-company/p/LethalCompanyModding/Yukieji_UnityExplorer/).
- Thunderstore for hosting this mod as I wouldn't know how to distribute without it.
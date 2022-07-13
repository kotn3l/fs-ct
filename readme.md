# FS-CT
This is a very poorly written converter tool for most generic FromSoftware formats, please absolutely don't use this unless you want to suffer (jk using it isn't so bad, but the code is pretty ugly). I will reformat and refactor someday so it strictly follows all coding conventions, but it is what it is for now. I only started making it since I desperately needed 3D models from Elden Ring Network Test Ver. in Dec 2021, but there was no tool available. (Well there was a tool but at the time it didn't extract the UVs right)

It decompresses and decrypts BNDs and converts FLVERs (geometry), TPFs (textures), MSGs (texts), MSBs (map placements), and can also kinda change the platform of a FS file.

## Model conversion
Whether it be chrbnd, or partbnd, or anything that contains an FLVER, it will convert it. Will also convert textures from the BND container. **Can also BATCH convert an entire folder of BNDs.**

## FULL map conversion

Select a map from the static list, and then convert that map to ASCII. Also supports textures, that will be in output in DDS. The geometry (except the base geometry) is grouped together on how many instances an object has on the map, and then converted. This is very bad practice, as it will create huge files, but alas, there's no other way to convert.

![Elphael](/img/elphael.png?raw=true "Elphael in Blender")

New Multithreaded option, performance tests were run on 1600X (6c/12t):

<center>

|Test ran on maps                |SingleThreaded       |MultiThreaded| MT faster by %
|----------------|-------------------------------|-----------------------------|-|
|**m10_01_00_00**|**4**m 28s 163ms            |3m 19s 573ms            | 34.2% |
|**m31_03_00_00**|**0**m 26s 816ms            |0m 20s 722ms            |29.4% |
|**m11_10_00_00**          |2m 14s 910ms|1m 9s 530ms| 94% |
|**m60_10_09_02**          |7m 38s 328ms|4m 27s 710ms| 71.2% |
|**AVERAGE**          ||| 57.2% |

</center>

## PLAYER CHARACTER MERGING AND CONVERSION
Now it can merge player model and armors/weapons, with a fully working skeleton.
![Player model merged skeleton](/img/bloodywolf_m.png?raw=true "Skeleton, and render")

# REQUIREMENTS
[SoulsFormats](https://github.com/JKAnderson/SoulsFormats) (er branch), oo2core_6_win64.dll

# CREDITS
[googleben's ERMapViewer](https://github.com/googleben/ERMapViewer) for partial map loading code
[SoulsFormats](https://github.com/JKAnderson/SoulsFormats)
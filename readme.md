## Map Conversion to ASCII with texture support

New Multithreaded option, performance thread was run on 1600X (6c/12t):

|Test ran on maps                |SingleThreaded       |MultiThreaded| MT faster by %
|----------------|-------------------------------|-----------------------------|-|
|**m10_01_00_00**|**4**m 28s 163ms            |3m 19s 573ms            | 34.2% |
|**m31_03_00_00**|**0**m 26s 816ms            |0m 20s 722ms            |29.4% |
|**m11_10_00_00**          |2m 14s 910ms|1m 9s 530ms| 94% |
|**m60_10_09_02**          |7m 38s 328ms|4m 27s 710ms| 71.2% |
|**AVERAGE**          ||| 57.2% |


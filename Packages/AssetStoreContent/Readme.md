# Universal Rename Tool 1.0.0

## Overview

It is an editor tool for Unity which allows you to replace object name parts with other values and works with multiple objects simultaneously.

## Example

You have a scene with hundreds of gameObjects with unique names and you need to rename part of them, e.g. with 'item' in name. Instead of changing all names ('item0', 'item1', etc) manually you can find them with Universal Rename Tool and replace 'item' with everything you want ('item\*' => 'instance\*').

## Features

* Search both in project & scenes
* Replace part of an object name with another part
* Ignore case option
* RegEx support
* Search through all scene or in particular object and its children
* Optional match limitation

## Usage guide
1. You can open **Rename Tool** window using **Window/Rename Tool** menu item.
![step_1](http://image.prntscr.com/image/db57f390350c4e86a11c5b491b4d6c48.png)
2. Opened window can be attached to interface panels like any other windows like **Inspector** or **Project** Window.
![step_2](http://image.prntscr.com/image/55dbed74206b47d3a170a8b069456239.png) ![step_3](http://image.prntscr.com/image/bad61fc80a48419bb9854026c4aeadcc.png)
3. You can select, where you need to find objects or assets: in Scene or in Project:
![step_4](http://image.prntscr.com/image/bc54e61e6e1d4f43b0f5bc3d72b6c086.png)
4. Also, if you want to find in concrete **GameObject** children on Scene, you can drag'n'drop or select it:
![step_5](http://image.prntscr.com/image/84fa4a47b2c04b2b87047a2aec742268.png)
5. After you start typing name to find in **Find** text field, suitable items will be selected in **Scene** or **Project** Window:
![step_6](http://image.prntscr.com/image/b9829444943f4446a7eb1efd67629881.png)
![step_7](http://image.prntscr.com/image/02d8b5b8ca2c461b83ea942facc073ba.png)
6. Then you can type what you want to replace in item name in **Replace** field and click **Rename** button. All suitable item name will change their names:
![step_8](http://image.prntscr.com/image/a30c7e5a82f24c9488a2654eb7ce1265.png)
7. Also, you can select **RegExp** mode and find complicated matches:
![step_9](http://image.prntscr.com/image/4637fdd1d780452f92a21a3cbbbfefa8.png)
![step_10](http://image.prntscr.com/image/7dd19da0fef04f22ae098356ddf79081.png)
8. Also, you can enable **Ignore case** and find items by name in any case:
![step_11](http://image.prntscr.com/image/f4009213ec6a407d8708b858727ce3f0.png)
9. Also, you can use **Count** field to replace only N first matches (in this example we want to change first "item", but second "item" required to doesn't changed):
![step_11](http://image.prntscr.com/image/79959c092f514b42a09d88118386e6b6.png)
![step_12](http://image.prntscr.com/image/39fc69393ab447c787cde2544109dd07.png)


# ModdersToolKit

ModdersToolKit, or MTK from now on is a fan-made SDK for the game "Lethal Company" using C# and MelonLoader.
Right now, this includes very bare bones code. Like adding items, adding sounds, "mixins" which the docs will get into, "hooks" which is another thing the docs will get into, and more eventually.

# Why?

The idea for this project was to make modding this game more accessible.
I also wanted to learn Unity modding using MelonLoader, so I made this.
Not the best code, but if you can fix something, submit a pull request.

# Restrictions?

Practically none - just don't sell the MTK.
I don't endorse cheats. You can use the MTK to make cheats,
but don't expect me to add anything that makes cheats easier.

# Who would want this?

I don't know. Wanna make a mod? Use the MTK! It'll be easy. I hope.

# Setup

Just download/compile the DLL, add as a refrence, and check out the Wiki for getting started.
To edit the mod, clone the repo, and if using Visual Studio, package all of the code into a folder called "ModdersToolKit"
In the same directory as this new folder, place the solution file. Then, open the solution and you should be good.

# To-do

- [X] Mixins (sorta, only injection is made.)
- [ ] Chat Commands
- [ ] Writing to chat
- [X] Reading from chat
- [X] Hooks
- [ ] Wiki
- [X] Sound Loading
- [ ] Model Loading
- [ ] Image Loading
- [ ] Shader loading
- [X] Custom Boombox Music
- [X] Custom items
- [X] Registering items to the terminal

# Dependencies

Harmony is one - though the mod DLL comes with it.
You can find it [here](https://github.com/pardeike/Harmony)

You also need the game (duh)

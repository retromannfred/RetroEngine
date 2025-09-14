# RetroEngine (WIP)

A C# game engine created from scratch using the ECS pattern (Entity-Component-System) and OpenTK library to manage graphics and audio, and built in a way to provide a modular enviorement, flexible and with a high performance in order to build 2D and 3D games.

## ❓ Why this project engine?

I created this project to adquire knowledge about:
- ECS pattern and proper handling of its components.
- CPU cache, GPU and grpahics memory manageing.
- How to work with OpenGL and its shaders.
- Maths on transformation matrices and physics.
- Audio and music in a game works.


## 🚀 Características
- Architecture based on **ECS** in order to separate data, logic and rendering.
- Rendering with **OpenTK** (OpenGL over .NET).
- Modularity: designing as a collection of independent modules, where each module handles a specific functionality and communicates with others thanks to ECS architecture.
- Reusability: easy to extend with new systems and components.


## 📦 Install and compile
``` bash
git clone https://github.com/retromannfred/RetroEngine
cd RetroEngine
dotnet build
```

Then you can run the **RetroEngine.FuncTest** to test some of the functions the engine have.


## 🕹️ Basic use

This is minimal example to create a window and render an entity:

``` C#
using OpenTK.Mathematics;
using RetroEngine.Core;
using RetroEngine.Graphics;

internal class MyFirstGame() : Game("My first game", 800, 600)
{
    private World _world;

    protected override void LoadContent()
    {
        _world = new WorldBuilder()
            .RegisterSystem(new SpriteSystem(GraphicSettings))
            .RegisterSystem(new CameraSystem(GraphicSettings))
            .Build();

        _world.CreateEntity()
            .Attach(new Transform())
            .Attach(new SpriteRenderer(TextureFactory.CreateCircle(100, Color4.White)));

        _world.CreateEntity()
            .Attach(new Transform() { Position = Vector3.UnitZ * 3 })
            .Attach(new Camera());
    }

    protected override void Update(GameTime time)
    {
        _world.Update(time);
    }

    protected override void Render(GameTime time)
    {
        ClearScreen(Color4.CornflowerBlue);
        _world.Render(time);
    }
}
```

And how to run it:

``` C#
internal class Program
{
    static void Main()
    {
        new MyFirstGame().Run();
    }
}
```


## 📂 Project structure
    /source
        ├ RetroEngine.Core      -> Base ECS with window managing
        ├ RetroEngine.Graphics  -> OpenTK rendering algon with shaders and camera
        ├ RetroEngine.Physics   -> Physics functionality and interaction between objects
        └ RetroEngine.Buddies   -> Helpers to visualize stuff while debbuging
    /testing
        ├ RetroEngine.FuncTest  -> Some test games to visualize features
        └ RetroEngine.UnitTest  -> Unit tests with TDD methodology

## 🤝 Want to contribute?

If you want to help me out building (enhancing or refactoring) anything, just follow this steps:

- Fork this repository in a new one.
- Check list of issues you think you can complete [here](https://github.com/retromannfred/RetroEngine/issues).
- Create a branch for your changes (e.g. `git checkout -b feature/issue-n`)-
- Open a new pull request.

If you're using the engine to build any game and you find any problem or something nice to have, report it [here](https://github.com/retromannfred/RetroEngine/issues/new/choose/).

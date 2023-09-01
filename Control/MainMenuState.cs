#region Usings

using SamSer.Components;
using SamSer.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using static SamSer.Utilities.GameUtilities;

#endregion

namespace SamSer.Control;

public sealed class MainMenuState : State
{
    #region Fields

    private const int MENU_X_COORDINATE = 650;
    private const int MENU_Y_COORDINATE = 320;
    private const int MENU_OFFSET = 100;
    private readonly List<Component> _components;
    private readonly List<Component> _loadComponents;
    private Texture2D _backgroundTexture;
    private Song song;
    private LevelController _levelController;
    private bool _loadSelectMenuVisible;

    #endregion
    #region Constructors

    public MainMenuState(ContentManager contentManager, TwoDPlatformer game, GraphicsDevice graphicsDevice)
        : base(contentManager, game, graphicsDevice)
    {
        _components = new List<Component>();
        _loadComponents = new List<Component>();
        LoadComponents();
        _loadSelectMenuVisible = false;
    }

    #endregion

    #region Content-Loading methods
    private void LoadComponents()
    {
        _levelController = new LevelController(_contentManager, _graphicsDevice);
        //textures
        _backgroundTexture = _contentManager.Load<Texture2D>("country-platform-back");
        var titleFont = _contentManager.Load<SpriteFont>("TitleFont");
        var buttonFont = _contentManager.Load<SpriteFont>("ButtonFont");
        var volumeFont = _contentManager.Load<SpriteFont>("VolSliderDescriptionFont");
        var buttonTexture = _contentManager.Load<Texture2D>("Button");
        var sliderTexture = _contentManager.Load<Texture2D>("Slider");
        var sliderScrollTexture = _contentManager.Load<Texture2D>("SliderScroll");

        var gameTitle = new TextComponent(titleFont, new Vector2(620, 100), "SamSer");


        var startGameButton = new Button(buttonTexture,
            buttonFont,
            new Vector2(MENU_X_COORDINATE, MENU_Y_COORDINATE),
            "New Game");
        startGameButton.ButtonPress += NewGame_OnClick;

        var loadGameButton = new Button(buttonTexture,
            buttonFont,
            new Vector2(MENU_X_COORDINATE, MENU_Y_COORDINATE + MENU_OFFSET),
            "Load Game");
        loadGameButton.ButtonPress += LoadMenu_OnClick;

        var exitButton = new Button(buttonTexture,
            buttonFont,
            new Vector2(MENU_X_COORDINATE, MENU_Y_COORDINATE + 2 * MENU_OFFSET),
            "Exit");
        exitButton.ButtonPress += ExitGame_OnClick;
        var volumeCounter = new Counter<float>(volumeFont, new Vector2(MENU_X_COORDINATE,
            MENU_Y_COORDINATE + 3 * MENU_OFFSET), "Volume", 100, () => (float)Math.Round(MediaPlayer.Volume * 100));
        var volumeSlider = new Slider(sliderTexture, sliderScrollTexture, new Vector2(MENU_X_COORDINATE,
            MENU_Y_COORDINATE + 4 * MENU_OFFSET));

        _components.Add(
            gameTitle,
            startGameButton,
            loadGameButton,
            exitButton,
            volumeSlider,
            volumeCounter);
        for (var i = 1; i < 5; i++)
        {
            var button = new SerialButton<int>(buttonTexture,
                buttonFont,
                new Vector2(200 + (buttonTexture.Width + 10f) * (i - 1), 200),
                $"Save {i}",
                i);
            button.ButtonPress += LoadSave_OnClick;
            _loadComponents.Add(button);
        }

        //sounds
        song = _contentManager.Load<Song>("Sounds/space");
        PlayMediaPlayer();
        volumeSlider.SliderScroll += VolumeSlider_VolumeChanged;
    }

    #endregion

    #region State methods

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0), Color.White);
        spriteBatch.End();
        spriteBatch.Begin();
        if (!_loadSelectMenuVisible) { _components.ForEach(c => c.Draw(gameTime, spriteBatch)); }
        else
        {
            _loadComponents.ForEach(c => c.Draw(gameTime, spriteBatch));
        }
        spriteBatch.End();
    }

    public override void Update(GameTime gameTime)
    {
        if (!_loadSelectMenuVisible) _components.ForEach(c => c.Update(gameTime));
        else
        {
            _loadComponents.ForEach(c => c.Update(gameTime));
        }
    }

    #endregion

    #region Content events
    private void LoadSave_OnClick<T>(object sender, T args)
    {
        var fileName = DIR_PATH_RELATIVE + $"Saves/save{args}";
        if (!File.Exists(fileName))
        {
            return;
        }
        var data = File.ReadAllText(fileName);
        var gameData = JsonConvert.DeserializeObject<GameData>(data);
        var playerData = gameData.PlayerData;
        var level = gameData.Level;
        _levelController.SetLevel(level);
        _game.ChangeState(new GameState(_contentManager, _game, _graphicsDevice, _levelController, playerData));
    }
    private void NewGame_OnClick(object sender, EventArgs e)
    {
        StopMediaPlayer();
        _game.ChangeState(new GameState(_contentManager, _game, _graphicsDevice, _levelController));
    }

    private static void VolumeSlider_VolumeChanged(object sender, float volume)
    {
        MediaPlayer.Volume = volume;
    }

    private void LoadMenu_OnClick(object sender, EventArgs e)
    {
        StopMediaPlayer();
        _loadSelectMenuVisible = !_loadSelectMenuVisible;
    }

    private void ExitGame_OnClick(object sender, EventArgs e)
    {
        StopMediaPlayer();
        _game.Exit();
    }


    #endregion

    #region MediaPlayer control methods

    private void PlayMediaPlayer()
    {
        MediaPlayer.Play(song);
        MediaPlayer.IsRepeating = true;
    }

    private static void StopMediaPlayer()
    {
        MediaPlayer.Stop();
    }

    #endregion
}
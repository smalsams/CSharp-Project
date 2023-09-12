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
using Microsoft.Xna.Framework.Input;
using static SamSer.Utilities.GameUtilities;
using SamSer.Entities;
using Newtonsoft.Json.Linq;

#endregion

namespace SamSer.Control;

public sealed class MainMenuState : State
{
    #region Fields
    /// <remarks>
    /// List of all <see cref="Component"/>s shown during the regular screen.
    /// </remarks>
    private readonly List<Component> _components;
    /// <remarks>
    /// List of all <see cref="Component"/>s shown during the loading screen.
    /// </remarks>
    private readonly List<Component> _loadComponents;

    private Texture2D _backgroundTexture;


    private Song _song;
    private LevelController _levelController;

    /// <remarks>
    /// Indicates whether the loading screen is visible.
    /// </remarks>
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
    /// <summary>
    /// Loads assets exclusive/non-exclusive for <see cref="MainMenuState"/>
    /// into the content manager and creates components that these assets represent.
    /// </summary>
    private void LoadComponents()
    {
        _levelController = new LevelController(ContentManager, GraphicsDevice);
        _levelController.WinGame += WinGame_OnFinishingLastLevel;
        //textures
        _backgroundTexture = ContentManager.Load<Texture2D>("country-platform-back");
        var titleFont = ContentManager.Load<SpriteFont>("TitleFont");
        var buttonFont = ContentManager.Load<SpriteFont>("ButtonFont");
        var volumeFont = ContentManager.Load<SpriteFont>("VolSliderDescriptionFont");
        var buttonTexture = ContentManager.Load<Texture2D>("Button");
        var sliderTexture = ContentManager.Load<Texture2D>("Slider");
        var sliderScrollTexture = ContentManager.Load<Texture2D>("SliderScroll");

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
        _song = ContentManager.Load<Song>("Sounds/space");
        PlayMediaPlayer();
        volumeSlider.SliderScroll += VolumeSlider_VolumeChanged;
    }

    #endregion

    #region State methods
    /// <inheritdoc/>
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
    /// <inheritdoc/>
    public override void Update(GameTime gameTime)
    {
        if (!_loadSelectMenuVisible) _components.ForEach(c => c.Update(gameTime));
        else
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {

                _loadSelectMenuVisible = false;
            }
            _loadComponents.ForEach(c => c.Update(gameTime));
        }
    }

    #endregion

    #region Content events

    /// <summary>
    /// Loads data from json save into its in-game representation
    /// </summary>
    /// <typeparam name="T">Type identifying the save file name/index </typeparam>
    /// <param name="sender">Reference to the object that raised the event</param>
    /// <param name="args">Object representing the save file name/index</param>
    /// <exception cref="ArgumentOutOfRangeException">Raised if the save file entity is not in the list of supported entities.</exception>
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
        _levelController.Current.Entities.Clear();
        foreach (var entityInfo in gameData.EntityData)
        {
            var properties = JObject.Parse(entityInfo.Properties);
            IEntity entity = entityInfo.Type switch
            {
                "Coin" => JsonConvert.DeserializeObject<Coin>(entityInfo.Properties) ,
                "FlameEnemy" => JsonConvert.DeserializeObject<FlameEnemy>(entityInfo.Properties),
                _ => throw new ArgumentOutOfRangeException()
            };
            var name = entity.GetTextureName(properties);
            entity.LoadTexture(_levelController.TextureManager[name]);
            _levelController.Current.Entities.Add(entity);
        }
        Game.ChangeState(new GameState(ContentManager, Game, GraphicsDevice, _levelController, playerData));
        _levelController.Current.Pause();
    }
    /// <summary>
    /// Creates new instance of the game, changes <see cref="State"/> to <see cref="GameState"/>.
    /// </summary>
    /// <param name="sender">Reference to the object that raised the event</param>
    /// <param name="e">Event data</param>
    private void NewGame_OnClick(object sender, EventArgs e)
    {
        StopMediaPlayer();
        Game.ChangeState(new GameState(ContentManager, Game, GraphicsDevice, _levelController));
    }
    /// <summary>
    /// Changes volume of the background music to the <see cref="float"/> number specified by UI logic.
    /// </summary>
    /// <param name="sender">Reference to the object that raised the event</param>
    /// <param name="volume"><see cref="float"/> number representing volume of the background music</param>
    private static void VolumeSlider_VolumeChanged(object sender, float volume)
    {
        MediaPlayer.Volume = volume;
    }
    /// <summary>
    /// Displays menu with save files and <see cref="Button"/>s representing them.
    /// </summary>
    /// <param name="sender">Reference to the object that raised the event</param>
    /// <param name="e">Event data</param>
    private void LoadMenu_OnClick(object sender, EventArgs e)
    {
        StopMediaPlayer();
        _loadSelectMenuVisible = !_loadSelectMenuVisible;
    }
    /// <summary>
    /// Exits the game and terminates the program.
    /// </summary>
    /// <param name="sender">Reference to the object that raised the event</param>
    /// <param name="e">Event data</param>
    private void ExitGame_OnClick(object sender, EventArgs e)
    {
        StopMediaPlayer();
        Game.Exit();
    }
    /// <summary>
    /// Changes the <see cref="State"/> to <see cref="EndState"/>, which represents the game ending.
    /// </summary>
    /// <param name="sender">Reference to the object that raised the event</param>
    /// <param name="e">Event data</param>
    private void WinGame_OnFinishingLastLevel(object sender, EventArgs e)
    {
        Game.ChangeState(new EndState(ContentManager, Game, GraphicsDevice));
    }

    #endregion

    #region MediaPlayer control methods
    /// <summary>
    /// Plays the background music for <see cref="MainMenuState"/>.
    /// </summary>
    private void PlayMediaPlayer()
    {
        MediaPlayer.Play(_song);
        MediaPlayer.IsRepeating = true;
    }
    /// <summary>
    /// Stops the background music for <see cref="MainMenuState"/>
    /// </summary>
    private static void StopMediaPlayer()
    {
        MediaPlayer.Stop();
    }

    #endregion
}
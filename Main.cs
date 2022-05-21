﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


using MidAgeRevolution.AllButton;
using MidAgeRevolution.AllScreen;
using MidAgeRevolution.AllSprite;
using MidAgeRevolution.AllSprite.AllPlayer;

namespace MidAgeRevolution
{
    public class Main : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;

        private List<Screen> _screen;
            
        Texture2D test, restart_tex2d, mainmenu_tex2d, rl_win_popup, sc_win_popup;
        private Button _res, _mm;
        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferWidth = Singleton.WINDOWS_SIZE_X;
            _graphics.PreferredBackBufferHeight = Singleton.WINDOWS_SIZE_Y;

            _graphics.ApplyChanges();


            _screen = new List<Screen>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            test = this.Content.Load<Texture2D>("Test/test0");
            Singleton.Instance.sc = this.Content.Load<Texture2D>("Asset/science");
            Singleton.Instance.rl = this.Content.Load<Texture2D>("Asset/religious_new");
            Singleton.Instance.sc_hp_bar = this.Content.Load<Texture2D>("Asset/sc_health_bar");
            Singleton.Instance.rl_hp_bar = this.Content.Load<Texture2D>("Asset/rl_health_bar");
            Singleton.Instance.screenBorder = this.Content.Load<Texture2D>("Asset/play_screen_border");
            Singleton.Instance.bg = this.Content.Load<Texture2D>("Asset/background");
            Singleton.Instance.sc_tw_02_v = this.Content.Load<Texture2D>("Asset/sc_tw_02_vertical");
            Singleton.Instance.sc_tw_02_h = this.Content.Load<Texture2D>("Asset/sc_tw_02_horizontal");
            Singleton.Instance.Arrow = this.Content.Load<Texture2D>("Test/Arrow");
            
            //main menu texture2D
            Singleton.Instance.menu_bg = this.Content.Load<Texture2D>("Asset/bg");
            Singleton.Instance.title = this.Content.Load<Texture2D>("Asset/title");
            Singleton.Instance.st_btn = this.Content.Load<Texture2D>("Asset/start");
            Singleton.Instance.tu_btn = this.Content.Load<Texture2D>("Asset/tutorial");
            Singleton.Instance.ex_btn = this.Content.Load<Texture2D>("Asset/exit");
            Singleton.Instance.ghibi = this.Content.Load<Texture2D>("Asset/animator");

            //tutorial texture2D
            Singleton.Instance.re_btn = this.Content.Load<Texture2D>("Asset/return");
            Singleton.Instance.tu_title = this.Content.Load<Texture2D>("Asset/tutorial_title");
            Singleton.Instance.tu_detail = this.Content.Load<Texture2D>("Asset/detail");
            Singleton.Instance.ghb = new Texture2D(GraphicsDevice, 1, 1);
            Singleton.Instance.ghb.SetData(new[] { Color.White });

            _spriteFont = this.Content.Load<SpriteFont>("Test/font0");

            Singleton.Instance.Content = Content;
            Singleton.Instance.GraphicsDevice = _graphics.GraphicsDevice;
            Singleton.Instance.testfont = this.Content.Load<SpriteFont>("Test/font0");

            //win texture2D pop up
            Singleton.Instance.mm_b = this.Content.Load<Texture2D>("Asset/mm_b");
            Singleton.Instance.re_b = this.Content.Load<Texture2D>("Asset/re_b");
            Singleton.Instance.sc_win = this.Content.Load<Texture2D>("Asset/sc_win");
            Singleton.Instance.rl_win = this.Content.Load<Texture2D>("Asset/rl_win");

            rl_win_popup = (Texture2D)Singleton.Instance.rl_win;
            sc_win_popup = (Texture2D)Singleton.Instance.sc_win;
            restart_tex2d = (Texture2D)Singleton.Instance.re_b;
            mainmenu_tex2d = (Texture2D)Singleton.Instance.mm_b;
            _res = new MenuButton(restart_tex2d)
            {
                Position = new Vector2(792, 578)
            };

            _res.onClick += _resClick;

            _mm = new MenuButton(mainmenu_tex2d)
            {
                Position = new Vector2(1042, 578)
            };

            _mm.onClick += _mmClick;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Singleton.Instance.PreviousMouse = Singleton.Instance.CurrentMouse;
            Singleton.Instance.CurrentMouse = Mouse.GetState();
            Singleton.Instance.PrevoiusKey = Singleton.Instance.CurrentKey;
            Singleton.Instance.CurrentKey = Keyboard.GetState();

            Singleton.Instance._prvGameState = Singleton.Instance._gameState;
            Singleton.Instance._gameState = Singleton.Instance._nextGameState;

            Singleton.Instance._time = gameTime;

            // TODO: Add your update logic here
            switch (Singleton.Instance._mainState)
            {
                case Singleton.MainState.start:
                    _screen.Add(new MenuScreen(this, test)); //0
                    _screen.Add(new GameScreen(this, test)); //1
                    _screen.Add(new Tutorial(this, test)); //2
                    Singleton.Instance._mainState = Singleton.MainState.mainMenu;
                    break;

                case Singleton.MainState.mainMenu:
                    _screen[0].Update(_screen[0], gameTime);
                    break;

                case Singleton.MainState.tutorial:
                    _screen[2].Update(_screen[2], gameTime);
                    break;

                case Singleton.MainState.gamePlay:
                    _screen[1].Update(_screen[1], gameTime);
                    //Singleton.Instance._mainState = Singleton.MainState.gameEnd;
                    break;

                case Singleton.MainState.gameEnd:
                    _screen[1].Update(_screen[1], gameTime);
                    _res.Update(gameTime);
                    _mm.Update(gameTime);
                    //Singleton.Instance._mainState = Singleton.MainState.mainMenu;
                    break;
            }

            Singleton.Instance.PreviousMouse = Singleton.Instance.CurrentMouse;
            Singleton.Instance.PrevoiusKey = Singleton.Instance.CurrentKey;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // TODO: Add your drawing code here
            switch (Singleton.Instance._mainState)
            {
                case Singleton.MainState.start:
                    break;
                case Singleton.MainState.mainMenu:
                    _screen[0].Draw(_spriteBatch, gameTime);

                    break;
                case Singleton.MainState.tutorial:
                    _screen[2].Draw(_spriteBatch, gameTime);

                    break;
                case Singleton.MainState.gamePlay:
                    _screen[1].Draw(_spriteBatch);

                    break;
                case Singleton.MainState.gameEnd:
                    _screen[1].Draw(_spriteBatch);
                    //_spriteBatch.DrawString(_spriteFont, _screen[1].label, new Vector2(100, 100), Color.Black);
                    _spriteBatch.DrawString(_spriteFont, _screen[1].label, new Vector2(100, 100), Color.Black, 0, Vector2.Zero, new Vector2(5, 5), SpriteEffects.None, 0f);
                    //win
                    if(Singleton.Instance._gameResult == Singleton.GameResult.LuckWin)
                    {
                        _spriteBatch.Draw(rl_win_popup, new Vector2(320, 216), Color.White);
                    } else
                    {
                        _spriteBatch.Draw(sc_win_popup, new Vector2(320, 216), Color.White);
                    }
                    _res.Draw(_spriteBatch);
                    _mm.Draw(_spriteBatch);
                    break;
                default:
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        private void _resClick(object sender, EventArgs e)
        {
            Singleton.Instance._nextGameState = Singleton.GameState.Setup;
            Singleton.Instance._mainState = Singleton.MainState.gamePlay;
            Singleton.Instance._gameResult = Singleton.GameResult.None;
        }
        private void _mmClick(object sender, EventArgs e)
        {
            Singleton.Instance._mainState = Singleton.MainState.mainMenu;
        }
    }
}

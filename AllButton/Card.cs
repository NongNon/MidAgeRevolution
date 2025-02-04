﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MidAgeRevolution.AllButton
{
    class Card : Button
    {
        private Texture2D _texture;
        private int[] luckSkill;
        private Color[] skillColor =
        {
            Color.White,
            Color.White,
            Color.White
        };
        private int CurrentSkill;
        private int PreviousSkill;
        private Random _random;


        public Card(Texture2D texture) : base(texture)
        {
            _texture = texture;
            CurrentSkill = -1;
            PreviousSkill = -1;
            luckSkill = new int[3];
            _random = new Random();
        }

        public override void Update(Button gameButton)
        {
            switch (Singleton.Instance._gameState)
            {
                case Singleton.GameState.Setup:
                    rect = new Rectangle((int)position.X, (int)position.Y, (int)field_size.X, (int)field_size.Y);
                    randomAmmo();
                    Singleton.Instance.ammo = Singleton.AmmoType.x1dmg;

                    break;
                case Singleton.GameState.WisdomTurn:
                    break;
                case Singleton.GameState.LuckTurn:
                    if (Singleton.Instance.CurrentMouse.LeftButton == ButtonState.Pressed &&
                    Singleton.Instance.PreviousMouse.LeftButton == ButtonState.Released &&
                    Singleton.Instance.CurrentMouse.Y < position.Y + field_size.Y &&
                    Singleton.Instance.CurrentMouse.Y > position.Y)
                    {
                        if (Singleton.Instance.CurrentMouse.X < position.X + Singleton.Instance.SKILL_SIZE &&
                            Singleton.Instance.CurrentMouse.X > position.X)
                        {
                            CurrentSkill = 0;
                            setAmmoLuck();
                        }
                        else if (Singleton.Instance.CurrentMouse.X < position.X + Singleton.Instance.SKILL_SIZE * 2 &&
                            Singleton.Instance.CurrentMouse.X > position.X + Singleton.Instance.SKILL_SIZE)
                        {
                            CurrentSkill = 1;
                            setAmmoLuck();
                        }
                        else if (Singleton.Instance.CurrentMouse.X < position.X + Singleton.Instance.SKILL_SIZE * 3 &&
                            Singleton.Instance.CurrentMouse.X > position.X + Singleton.Instance.SKILL_SIZE * 2)
                        {
                            CurrentSkill = 2;
                            setAmmoLuck();
                        }
                    }

                    break;
                case Singleton.GameState.WisdomShooting:
                    break;
                case Singleton.GameState.LuckShooting:
                    break;
                case Singleton.GameState.WisdomEndTurn:
                    //randomAmmo();

                    break;
                case Singleton.GameState.LuckEndTurn:
                    for (int i = 0; i < 3; i++)
                    {
                        skillColor[i] = Color.White;
                    }
                    CurrentSkill = -1;
                    PreviousSkill = -1;
                    Singleton.Instance.ammo = Singleton.AmmoType.x1dmg;
                    randomAmmo();

                    break;
            }

            base.Update(gameButton);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (Singleton.Instance._gameState)
            {
                case Singleton.GameState.Setup:
                    break;
                case Singleton.GameState.WisdomTurn:
                    break;
                case Singleton.GameState.LuckTurn:
                    for (int i = 0; i < 3; i++)
                    {
                        spriteBatch.Draw(Singleton.Instance.cd_card, new Rectangle(rect.X + (i * rect.Width), rect.Y, Singleton.Instance.cd_card.Width, Singleton.Instance.cd_card.Height), skillColor[i]);
                        //spriteBatch.DrawString(Singleton.Instance.testfont, "" + luckSkill[i], new Vector2(position.X + 60 * i, 265), Color.Black, 0, Vector2.Zero, new Vector2(5, 5), SpriteEffects.None, 0f);
                    }

                    break;
                case Singleton.GameState.WisdomShooting:
                    break;
                case Singleton.GameState.LuckShooting:
                    break;
                case Singleton.GameState.WisdomEndTurn:
                    break;
                case Singleton.GameState.LuckEndTurn:
                    break;
            }

            base.Draw(spriteBatch);
        }

        private void setAmmoLuck()
        {
            if (PreviousSkill == -1)
            {
                skillColor[CurrentSkill] = Color.LightBlue;
                modAmmoLuck();
                PreviousSkill = CurrentSkill;
            }
            else if (CurrentSkill != PreviousSkill)
            {
                skillColor[CurrentSkill] = Color.LightBlue;
                skillColor[PreviousSkill] = Color.White;
                modAmmoLuck();
                PreviousSkill = CurrentSkill;
            }
            else if (CurrentSkill == PreviousSkill)
            {
                skillColor[CurrentSkill] = Color.White;
                CurrentSkill = -1;
                Singleton.Instance.ammo = Singleton.AmmoType.x1dmg;
            }
            else
            {
                Singleton.Instance.ammo = Singleton.AmmoType.x1dmg;
            }

            PreviousSkill = CurrentSkill;
        }

        private void modAmmoLuck()
        {
            switch (luckSkill[CurrentSkill])
            {
                case 1:
                    Singleton.Instance.ammo = Singleton.AmmoType.bounceBullet | Singleton.AmmoType.applyPhysics;
                    break;
                case 2:
                    Singleton.Instance.ammo = Singleton.AmmoType.xrndBdmg | Singleton.AmmoType.xrndammo;
                    break;
                case 3:
                    Singleton.Instance.ammo =  Singleton.AmmoType.xrndAdmg;
                    break;
                case 4:
                    Singleton.Instance.ammo = Singleton.AmmoType.xrndBdmg | Singleton.AmmoType.bounceBullet | Singleton.AmmoType.xrndammo;
                    break;
                case 5:
                    Singleton.Instance.ammo = Singleton.AmmoType.xrndBdmg | Singleton.AmmoType.turnOffWind;
                    break;
                case 6:
                    Singleton.Instance.ammo = Singleton.AmmoType.fire_debuf | Singleton.AmmoType.xrndammo;
                    break;
                case 7:
                    Singleton.Instance.ammo = Singleton.AmmoType.otk | Singleton.AmmoType.useItem;
                    break;
            }
        }

        private void randomAmmo()
        {
            List<int> rankB = new List<int>();
            rankB.Add(0);
            rankB.Add(1);
            rankB.Add(2);
            List<int> rankA = new List<int>();
            rankA.Add(0);
            rankA.Add(1);
            rankA.Add(2);
            int jackpot = 1;
            int rank;
            int type;
            for (int i = 0; i < 3; i++)
            {
                rank = _random.Next(0, 99 + jackpot);
                if (rank < 49)
                {
                    //B
                    type = _random.Next(0, rankB.Count);
                    switch (rankB[type])
                    {
                        case 0:
                            //Singleton.Instance.ammo = Singleton.AmmoType.normal;
                            luckSkill[i] = 1;
                            break;
                        case 1:
                            //Singleton.Instance.ammo = Singleton.AmmoType.x0dmg;
                            luckSkill[i] = 2;
                            break;
                        case 2:
                            //Singleton.Instance.ammo = Singleton.AmmoType.x0p5dmg;
                            luckSkill[i] = 3;
                            break;
                    }
                    rankB.RemoveAt(type);
                }
                else if (rank < 99)
                {
                    //A
                    type = _random.Next(0, rankA.Count);
                    switch (rankA[type])
                    {
                        case 0:
                            //Singleton.Instance.ammo = Singleton.AmmoType.x1p5dmg;
                            luckSkill[i] = 4;
                            break;
                        case 1:
                            //Singleton.Instance.ammo = Singleton.AmmoType.x2dmg;
                            luckSkill[i] = 5;
                            break;
                        case 2:
                            //Singleton.Instance.ammo = Singleton.AmmoType.x3dmg;
                            luckSkill[i] = 6;
                            break;
                    }
                    rankA.RemoveAt(type);
                }
                else
                {
                    //S
                    //Singleton.Instance.ammo = Singleton.AmmoType.otk;
                    luckSkill[i] = 7;
                    jackpot = 0;
                }
            }
        }
    }
}

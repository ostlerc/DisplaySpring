using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace GMA.Menus
{
    class OptionButton : MultiTextureButton
    {
        private Texture2D m_rightArrow;
        private Texture2D m_leftArrow;
        private Texture2D m_rightArrowFocus;
        private Texture2D m_leftArrowFocus;
        private SoundEffect OptionChangeSound = null;

        public MenuButton leftArrowButton;
        public MenuButton rightArrowButton;
        private List<int> m_lockedIndicies;

        private int m_index = 0;
        private int m_startIndex = 0;
        private bool m_locked = false;

        public delegate void MenuButtonAction(MenuButton optionButton);
        public MenuButtonAction OptionChanged = null;

        /// <summary>
        /// Used for displaying the arrows for only a small window of time
        /// Internal use only
        /// </summary>
        private double m_elapsed = -1;

        /// <summary>
        /// This holds all the options (aka buttons) in the option button
        /// All associated events and delegates are activated on button
        /// </summary>
        private List<MenuButton> m_options;

        /// <summary>
        /// This is the option index which is the default visible button
        /// </summary>
        public virtual int StartIndex
        {
            get { return m_startIndex; }
            set { m_startIndex = value; Reset(true); }
        }

        public override float Alpha
        {
            get { return base.Alpha; }
            set { base.Alpha = value; }
        }

        public override float EndAlpha
        {
            get { return base.EndAlpha; }
            set
            {
                foreach (MenuButton mb in m_options)
                    mb.EndAlpha = value;

                base.EndAlpha = value;
            }
        }

        public override SpriteFont Font
        {
            get { return base.Font; }
            set
            {
                foreach (MenuButton mb in m_options)
                    mb.Font = value;

                base.Font = value;
            }
        }

        public override Color FontColor
        {
            get { return base.FontColor; }
            set
            {
                foreach (MenuButton mb in m_options)
                    mb.FontColor = value;

                base.FontColor = value;
            }
        }

        public override Color fontFocusColor
        {
            get { return base.fontFocusColor; }
            set
            {
                foreach (MenuButton mb in m_options)
                    mb.fontFocusColor = value;

                base.fontFocusColor = value;
            }
        }

        public override MultiController ItemController
        {
            get { return base.ItemController; }
            set
            {
                foreach (MenuButton mb in m_options)
                    mb.ItemController = value;

                base.ItemController = value;
            }
        }

        public override float StartAlpha
        {
            get { return base.StartAlpha; }
            set
            {
                foreach (MenuButton mb in m_options)
                    mb.StartAlpha = value;

                base.StartAlpha = value;
            }
        }

        public override float TextScale
        {
            get { return base.TextScale; }
            set { base.TextScale = value; }
        }

        public bool Wrap = true;

        /// <summary>
        /// A locked OptionButton will not change its index
        /// </summary>
        public virtual bool Locked
        {
            get { return m_locked; }
            set { m_locked = value; }
        }

        public override AnimateType SelectedAnimation
        {
            set
            {
                foreach (MenuButton mb in m_options)
                    mb.SelectedAnimation = value;

                base.SelectedAnimation = value;
            }
        }

        public void ClearLocks()
        {
            m_lockedIndicies.Clear();
        }

        /// <summary>
        /// Represents that an index has been chosen
        /// If this object is not locked and also is on that
        /// particular index, it will choose a different index
        /// </summary>
        /// <param name="?"></param>
        public void ChosenIndex(int index)
        {
            if (Locked)
                return;

            if (!m_lockedIndicies.Contains(index))
            {
                m_lockedIndicies.Add(index);
            }

            if (m_index != index)
                return;

            //find a not locked index and stick with it.
            for (int x = 0; x < m_options.Count; x++)
            {

                if (!m_lockedIndicies.Contains(x))
                {
                    Index = x;
                }
            }
        }

        /// <summary>
        /// releases lock on index
        /// </summary>
        /// <param name="index"></param>
        public void UnchosenIndex(int index)
        {
            if (Locked)
                return;

            m_lockedIndicies.Remove(index);
        }

        /// <summary>
        /// Changing the index will swich which option is shown on the button
        /// If locked setting the index will do nothing
        /// </summary>
        public int Index
        {
            get { return m_index; }
            set
            {
                if (Locked)
                    return;

                bool wasEnabled = false;
                bool wasVisible = false;

                if (m_options.Count() > 0)
                {
                    wasEnabled = m_options[m_index].Enabled;
                    wasVisible = m_options[m_index].Visible;
                    m_options[m_index].Enabled = false;
                    m_options[m_index].Visible = false;
                }

                if (value == -2)
                    return;

                if (value >= m_options.Count)
                {
                    if (Wrap)
                        m_index = 0;
                }
                else if (value < 0)
                {
                    if (Wrap)
                        m_index = m_options.Count - 1;
                }
                else
                    m_index = value;

                if (m_options.Count() > m_index)
                {
                    m_options[m_index].Enabled = wasEnabled;
                    m_options[m_index].Visible = wasVisible;

                    if (OptionChanged != null)
                    {
                        OptionChanged(m_options[m_index]);
                    }

                    SetArrowTranslate();
                }
            }
        }

        public MenuButton CurrentOption()
        {
            if (m_options.Count() > m_index)
            {
                return m_options[m_index];
            }

            return null;
        }

        private void IncrementIndex()
        {
            for (int x = m_index+1; x < m_options.Count; x++)
                if (!m_lockedIndicies.Contains(x))
                {
                    Index = x;
                    return;
                }

            if(Wrap)
                for (int x = 0; x < m_index; x++)
                    if (!m_lockedIndicies.Contains(x))
                    {
                        Index = x;
                        return;
                    }
        }

        private void DecrementIndex()
        {
            for (int x = m_index-1; x >= 0; x--)
                if (!m_lockedIndicies.Contains(x))
                {
                    Index = x;
                    return;
                }

            if(Wrap)
                for (int x = m_options.Count-1; x >= m_index; x--)
                    if (!m_lockedIndicies.Contains(x))
                    {
                        Index = x;
                        return;
                    }
        }

        public OptionButton(MultiController c, List<string> options, Texture2D leftArrow, Texture2D rightArrow, Texture2D leftArrowFocus, Texture2D rightArrowFocus, Texture2D background, Texture2D focused) 
            :base(c, null)
        {
            m_options = new List<MenuButton>();

            foreach (string s in options)
            {
                m_options.Add(new MenuButton(c, background, focused, s));
            }

            Init(c, m_options, leftArrow, rightArrow, leftArrowFocus, rightArrowFocus, background, focused, new List<int>());
        }

        public OptionButton(MultiController c, List<MenuButton> options, Texture2D leftArrow, Texture2D rightArrow, Texture2D leftArrowFocus, Texture2D rightArrowFocus) 
            :base(c, null)
        {
            Init(c, options, leftArrow, rightArrow, leftArrowFocus, rightArrowFocus, null, null, new List<int>());
        }

        public OptionButton(MultiController c, List<MenuButton> options)
            : base(c, null)
        {
            Init(c, options, null, null, null, null, null, null, new List<int>());
        }

        public OptionButton(MultiController c, List<MenuButton> options, ref List<int> usedIndices)
            : base(c, null)
        {
            Init(c, options, null, null, null, null, null, null, usedIndices);
        }

        public OptionButton(MultiController c, string text, Texture2D leftArrow, Texture2D rightArrow, Texture2D leftArrowFocus, Texture2D rightArrowFocus, Texture2D background, Texture2D focused)
            :base(c, null)
        {
            m_options = new List<MenuButton>();
            m_options.Add(new MenuButton(c, background, focused, text));

            Init(c, m_options, leftArrow, rightArrow, leftArrowFocus, rightArrowFocus, null, null, new List<int>());
        }

        private void SetArrowTranslate()
        {
            if (leftArrowButton != null)
            {
                if (m_options[Index].Width < leftArrowButton.Width * 6)
                    leftArrowButton.Translate = new Vector2(-m_options[Index].Width - leftArrowButton.Width, 0) / 2;
                else
                    leftArrowButton.Translate = new Vector2(-m_options[Index].Width + leftArrowButton.Width, 0) / 2;
            }

            if (rightArrowButton != null)
            {
                if (m_options[Index].Width < rightArrowButton.Width * 6)
                    rightArrowButton.Translate = new Vector2(m_options[Index].Width + rightArrowButton.Width, 0) / 2;
                else
                    rightArrowButton.Translate = new Vector2(m_options[Index].Width - rightArrowButton.Width, 0) / 2;
            }
        }

        private void Init(MultiController c, List<MenuButton> options, Texture2D leftArrow, Texture2D rightArrow, Texture2D leftArrowFocus, Texture2D rightArrowFocus, Texture2D background, Texture2D focused, List<int> lockedIndices)
        {
            OptionChangeSound = MenuItem.DefaultFocusSound;
            this.m_rightArrow = rightArrow;
            this.m_leftArrow = leftArrow;
            this.m_leftArrowFocus = leftArrowFocus;
            this.m_rightArrowFocus = rightArrowFocus;
            this.m_options = options;
            this.m_lockedIndicies = lockedIndices;

            if (null == background)
            {
                if (m_options.Count() > 0)
                    background = m_options[0].Background;
            }

            if (null == focused)
            {
                if (m_options.Count() > 0)
                    focused = m_options[0].FocusTexture;
            }

            m_buttons = new List<MenuButton>();


            if (null != leftArrow)
            {
                leftArrowButton = new MenuButton(c, leftArrow, null);

                leftArrowButton.LayerDepth -= 0.001f;
            }

            if (null != rightArrow)
            {
                rightArrowButton = new MenuButton(c, rightArrow, null);

                rightArrowButton.LayerDepth -= 0.001f;
            }


            foreach (MenuButton menuButton in options)
            {
                menuButton.Enabled = false;
                menuButton.Visible = false;
            }

            if (null != leftArrow)
            {
                leftArrowButton.HasFocus = false;
                m_buttons.Add(leftArrowButton);
            }
            if (null != rightArrow)
            {
                rightArrowButton.HasFocus = false;
                m_buttons.Add(rightArrowButton);
            }

            Reset(false);
            Visible = true;
        }

        public override float Height
        {
            get
            {
                if (m_options.Count > Index)
                    return m_options[Index].Height * ImageScale;
                else
                    return base.Height;
            }
        }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                if (m_options != null && m_options.Count > Index)
                    m_options[Index].Text = value;

                base.Text = value;
            }
        }

        public override float Width
        {
            get
            {
                float left = -1f, right = -1f;

                foreach (MenuButton mb in m_buttons.Union(m_options))
                {
                    if (mb.Pos.X + mb.Translate.X - mb.Width / 2 < left || left == -1)
                        left = mb.Pos.X + mb.Translate.X - mb.Width / 2;

                    if (mb.Pos.X +  mb.Translate.X + mb.Width / 2 > right || right == -1)
                        right = mb.Pos.X + mb.Translate.X + mb.Width / 2;
                }

                return (right - left) * ImageScale;
            }
        }

        public override float ImageScale
        {
            get { return base.ImageScale; }
            set
            {
                base.ImageScale = value;
                SetArrowTranslate();
            }
        }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                if (m_options.Count > Index)
                    m_options[Index].Enabled = value;

                base.Enabled = value;
            }
        }

        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                if (m_options.Count > Index)
                    m_options[Index].Visible = value;

                base.Visible = value;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (m_elapsed >= 0)
            {
                if(m_elapsed == 0 && HasFocus && OptionChangeSound != null)
                    OptionChangeSound.Play(0.5f, 0.0f, 0.0f);

                m_elapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

                if (m_elapsed >= 150)
                {
                    if (null != m_leftArrow)
                        leftArrowButton.FocusTexture = null;
                    if (null != m_rightArrow)
                        rightArrowButton.FocusTexture = null;
                    m_elapsed = -1;
                }
            }

            if (ItemController != null)
            {
                if (ItemController.Right)
                {
                    IncrementIndex();

                    if (null != m_rightArrow)
                        rightArrowButton.FocusTexture = m_rightArrowFocus;

                    m_elapsed = 0;
                }
                else if (ItemController.Left)
                {
                    DecrementIndex();
                    
                    if (null != m_leftArrow)
                        leftArrowButton.FocusTexture = m_leftArrowFocus;

                    m_elapsed = 0;
                }
            }

            foreach (MenuButton mb in m_options)
                mb.Update(gameTime);

            base.Update(gameTime);

            if (Initializing)
                return;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, MenuButtonProperties properties)
        {
            if (!Visible)
                return;

            m_options[Index].Draw(gameTime, spriteBatch, ItemProperties + properties);

            base.Draw(gameTime, spriteBatch, properties);
        }

        public override bool HasFocus
        {
            get { return base.HasFocus; }
            set
            {
                if (m_options.Count > Index)
                    m_options[Index].Enabled = value;

                base.HasFocus = value;
            }
        }

        public override void initialFocus()
        {
            if (m_options.Count > Index)
                m_options[Index].Enabled = true;

            base.initialFocus();
        }

        public override void Reset(bool isFocus)
        {
            Index = StartIndex;

            if (m_options.Count > Index)
                m_options[Index].Reset(isFocus);

            SetArrowTranslate();

            base.Reset(isFocus);
        }
    }
}

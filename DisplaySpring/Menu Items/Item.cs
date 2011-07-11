namespace DisplaySpring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Audio;

    using VAlign = Item.VerticalAlignmentType;
    using HAlign = Item.HorizontalAlignmentType;
    using Layout = Frame.LayoutType;
    using Microsoft.Xna.Framework.Input;
    /// <summary>
    /// An Item is any sub menu area that needs to interact with other menu parts.
    /// All items are shown in one menu
    /// </summary>
    public abstract class Item
    {
        #region Member variables

        /// <summary>
        /// Horizontal alignment types
        /// </summary>
        public enum HorizontalAlignmentType
        {
            /// <summary>
            /// Left align
            /// </summary>
            Left,

            /// <summary>
            /// Right align
            /// </summary>
            Right,

            /// <summary>
            /// Center align
            /// </summary>
            Center,

            /// <summary>
            /// Stretch contents Width to fit in given space
            /// </summary>
            Stretch
        };

        /// <summary>
        /// Center of the item. Cannot be changed
        /// </summary>
        public Vector2 Center
        {
            get
            {
                return new Vector2(StaticWidth, StaticHeight) / 2;
            }
        }

        /// <summary>
        /// Vertical alignment types
        /// </summary>
        public enum VerticalAlignmentType
        {
            /// <summary>
            /// Top align
            /// </summary>
            Top,

            /// <summary>
            /// Bottom align
            /// </summary>
            Bottom,

            /// <summary>
            /// Center align
            /// </summary>
            Center,

            /// <summary>
            /// Stretch contents Height to fit in given space
            /// </summary>
            Stretch
        };

        private uint m_layoutStretch = 1;
        private HorizontalAlignmentType m_horizontalAlignment = HAlign.Center;
        private VerticalAlignmentType m_verticalAlignment = VAlign.Center;
        private Vector2 m_center = Vector2.Zero;
        private ButtonState m_buttonState = ButtonState.Pressed;

        /// <summary>
        /// Default SelectSound
        /// </summary>
        public static SoundEffect DefaultSelectSound;
        /// <summary>
        /// Default CancelSound
        /// </summary>
        public static SoundEffect DefaultCancelSound;
        /// <summary>
        /// Default FocusSound
        /// </summary>
        public static SoundEffect DefaultFocusSound;
        
        /// <summary>
        /// Default Background for Buttons
        /// You still have to set this in your Button constructor
        /// </summary>
        public static Texture2D ButtonTexture;
        /// <summary>
        /// Default Focus Background (Highlight background) for Buttons
        /// </summary>
        public static Texture2D ButtonHighlightTexture;
        /// <summary>
        /// Default Left Arrow for OptionButton
        /// </summary>
        public static Texture2D ArrowLeft;
        /// <summary>
        /// Default Right Arrow for OptionButtons
        /// </summary>
        public static Texture2D ArrowRight;
        /// <summary>
        /// Default Left Highlight (Focused) Arrow for OptionButtons
        /// </summary>
        public static Texture2D DefaultArrowLeftHighlight;
        /// <summary>
        /// Default Right Highlight (Focused) Arrow for OptionButtons
        /// </summary>
        public static Texture2D DefaultArrowRightHighlight;

        internal float m_width;
        private float m_height;

        internal Vector2 m_pos = Vector2.Zero;
        internal AnimateType m_animation = AnimateType.None;
        internal bool m_fade = false;
        internal Animation m_alpha;
        internal SoundEffect m_focusSound;
        internal SoundEffect m_cancelSound;
        internal SoundEffect m_activateSound;
        internal bool m_visible = true;
        internal Color m_tint = Color.White;
        internal object m_tag = null;
        internal bool m_keepFocus = false;
        internal bool m_forceCancelSound = false;
        private double m_animCycleTime = 950; //this is in milliseconds
        internal ItemCollection m_ItemCollection;

        private MultiController m_controller;
        private Vector2 m_scale = Vector2.One;
        private int m_framesToActivate = 5;
        private int m_framesRun = 0;
        private float m_depth = Menu.ItemDrawDepth;
        private float m_rotation = 0;

        /// <summary>
        /// Do not use this directly. Use the property Focus instead.
        /// Failure to do so will result in undefined focus behavior
        /// </summary>
        private bool m_Focus = false;

        #endregion

        #region Delegates
        /// <summary>
        /// This delegate is called when the Item with Focus receives a 'Right' motion
        /// </summary>
        public MenuAction OnRight;

        /// <summary>
        /// This delegate is called when the Item with Focus receives a 'Left' motion
        /// </summary>
        public MenuAction OnLeft;

        /// <summary>
        /// This delegate is called when the Item with Focus receives a 'Up' motion
        /// </summary>
        public MenuAction OnUp;

        /// <summary>
        /// This delegate is called when the Item with Focus receives a 'Down' motion
        /// </summary>
        public MenuAction OnDown;

        /// <summary>
        /// This delegate is called when the Item with Focus receives a button press of 'A'
        /// </summary>
        public MenuAction OnA;

        /// <summary>
        /// This delegate is called when the Item with Focus receives a button press of 'B'
        /// </summary>
        public MenuAction OnB;

        /// <summary>
        /// This delegate is called when the Item with Focus receives a button press of 'X'
        /// </summary>
        public MenuAction OnX;

        /// <summary>
        /// This delegate is called when the Item with Focus receives a button press of 'Y'
        /// </summary>
        public MenuAction OnY;

        /// <summary>
        /// This delegate is called when the Item with Focus receives a button press of 'Start'
        /// </summary>
        public MenuAction OnStart;

        /// <summary>
        /// This delegate is called when the Item with Focus receives a button press of 'Back'
        /// ('Back' is also known as 'Select')
        /// </summary>
        public MenuAction OnBack;

        /// <summary>
        /// This delegate is called when the Item receives Focus
        /// </summary>
        public MenuAction OnFocus;

        /// <summary>
        /// This delegate is called when the Item loses Focus
        /// </summary>
        public MenuAction OnLoseFocus;
        #endregion

        #region Properties

        /// <summary>
        /// State in which triggers the On delegates
        /// </summary>
        public ButtonState InputState
        {
            get { return m_buttonState; }
            set { m_buttonState = value; }
        }

        /// <summary>
        /// Will force a refresh on this item.
        /// If it has a parent, the parent will
        /// be refreshed as well
        /// </summary>
        protected void forceRefresh()
        {
            if(Parent != null)
                Parent.refreshItem();
            else
                refreshItem();
        }

        /// <summary>
        /// Used in association with Parent
        /// </summary>
        private Item m_parent;

        /// <summary>
        /// Parent item of the object. The highest parent will always be the BaseFrame of the Menu.
        /// </summary>
        public Item Parent
        {
            get { return m_parent; }

            set
            {
                Item oldParent = m_parent;

                if (m_parent == value)
                    return;

                m_parent = value;

                if (oldParent != null)
                {
                    oldParent.Children.Remove(this);
                    oldParent.forceRefresh();
                }

                if (value != null && !value.Children.Contains(this))
                {
                    value.Children.Add(this);
                    //re-evaluate height and width
                }

                if (ItemController == null && value != null && value.ItemController != null)
                    ItemController = value.ItemController;

                forceRefresh();
            }
        }
        internal virtual ItemCollection Children { get { return m_ItemCollection; } set { m_ItemCollection = value; } }

        /// <summary>
        /// Used to know the stretch percentage of a Item when SizeType is Maximum.
        /// Default is 1. This number is taken relative to other Items in a layout.
        /// It determines the percentage of space given to the item.
        /// Example: a layout of two items with stretch 1 and 2 would receive 33% and 66%
        /// of the space respectively
        /// </summary>
        public virtual uint LayoutStretch 
        { 
            get { return m_layoutStretch; } 
            set 
            {
                if (m_layoutStretch != value)
                {
                    m_layoutStretch = value;
                    forceRefresh();
                }
            }
        }

        /// <summary>
        /// Get or Set the horizontal alignment for this item. Center is default
        /// Setting this might reset Scale value
        /// </summary>
        public HorizontalAlignmentType HorizontalAlignment 
        { 
            get { return m_horizontalAlignment; } 
            set 
            {
                if (m_horizontalAlignment == value)
                    return;

                m_horizontalAlignment = value;

                forceRefresh();
            }
        }

        /// <summary>
        /// Get or Set the vertical alignment for this item. Center is default
        /// Setting this might reset Scale value
        /// </summary>
        public VerticalAlignmentType VerticalAlignment 
        { 
            get { return m_verticalAlignment; } 
            set 
            { 
                if (m_verticalAlignment == value)
                    return;

                m_verticalAlignment = value;

                forceRefresh();
            }
        }

        /// <summary>
        /// If set to true, object will keep Focus in an invoke cycle
        /// This property resets itself to false at the end of the cycle
        /// </summary>
        public virtual bool KeepFocus
        {
            get { return m_keepFocus; }
            set { m_keepFocus = value; }
        }

        /// <summary>
        /// This is true if the item is ready for user input
        /// </summary>
        protected bool ReadyInput { get { return m_framesRun >= m_framesToActivate; } }

        /// <summary>
        /// If set to true, object will force a cancel sound even if focus is still true
        /// This is mostly to be used in association with KeepFocus
        /// This property resets itself to false after being used up
        /// </summary>
        public bool ForceCancelSound 
        { 
            get { return m_forceCancelSound; }
            set { m_forceCancelSound = value; }
        }
        
        /// <summary>
        /// Gets or sets the focus for the menu item
        /// On set to true the OnFocus delegate will be called and FocusSound will play
        /// </summary>
        public virtual bool Focus 
        { 
            get { return m_Focus; } 
            set
            {
                if (!m_Focus && value)
                {
                    if (FocusSound != null)
                        FocusSound.Play(0.5f, 0.0f, 0.0f);
                    if (OnFocus != null) //intentionally not using Invoke to not lose focus of item
                        OnFocus();

                    m_framesRun = 0; //give some time to reset input
                }
                else if (m_Focus && !value && OnLoseFocus != null) //losing focus
                    OnLoseFocus();

                m_Focus = value; 
            } 
        }

        /// <summary>
        /// A disabled button will not process in the update function
        /// This is a simple way to set focus to false without any consequence
        /// or side effect (like playing sounds, or causing delegates to be called)
        /// </summary>
        public virtual bool Enabled
        {
            get { return m_Focus; }
            set 
            { 
                m_Focus = value;
                if (value && OnFocus != null)
                {
                    OnFocus();
                    m_framesRun = 0;
                }
            }
        }

        /// <summary>
        /// Toggles the fading behavior of the Item. If set to true the item will fade in
        /// Default is false
        /// </summary>
        public virtual bool Fade
        {
            get { return m_fade; } 
            set
            {
                m_fade = value;

                if (!value)
                    m_alpha.Val = EndAlpha;
                else
                    m_alpha.Val = StartAlpha;
            }
        }

        /// <summary>
        /// Time item will fade in milliseconds if Fade is true
        /// </summary>
        public int FadeTime
        {
            get { if (m_alpha != null) return m_alpha.TimeoutMS; return Menu.DefaultFadeTime; }
            set { m_alpha.TimeoutMS = value; }
        }

        /// <summary>
        /// Scale of the object
        /// Default of 1
        /// </summary>
        public virtual Vector2 Scale { get { return m_scale; } set { m_scale = value; } }

        /// <summary>
        /// Rotation of the object in radians
        /// </summary>
        public virtual float Rotation { get { return m_rotation; } set { m_rotation = value; } }

        /// <summary>
        /// Transparency of the object. 0 -> 1 = transparent -> visible
        /// </summary>
        public virtual float Alpha { get { return m_alpha.Val; } set { m_alpha.Val = value; } }

        /// <summary>
        /// Transparency of the object with parents alpha taken into account
        /// </summary>
        public virtual float ScreenAlpha { get { return m_alpha.Val * (Parent == null ? 1 : Parent.ScreenAlpha); } }

        /// <summary>
        /// Position of the object. Default is Vector2.Zero, which is center based.
        /// </summary>
        public virtual Vector2 Position { get { return m_pos; } set { m_pos = value; } }

        private Vector2 m_layoutPosition = Vector2.Zero;
        private Vector2 m_offset = Vector2.Zero;

        /// <summary>
        /// Secondary position used for layouts. This is applied in conjunctionwith alignments.
        /// </summary>
        internal virtual Vector2 LayoutPosition { get { return m_layoutPosition; } set { m_layoutPosition = value; } }

        /// <summary>
        /// Primary way to 'offset' a position of an item with alignments. This is because position is reset when alignments are set.
        /// </summary>
        public virtual Vector2 Offset { get { return m_offset; } set { m_offset = value; refreshItem(); } }

        /// <summary>
        /// Animation type of the item
        /// </summary>
        public virtual AnimateType Animation { get { return m_animation; } set { m_animation = value; } }

        /// <summary>
        /// Height of the item. Layout space is included. Scale is not included.
        /// </summary>
        public virtual float Height 
        { 
            get 
            {
                return m_height;
            } 
            internal set 
            {
                if (m_height != value)
                {
                    m_height = value;
                    forceRefresh();
                }
            } 
        }

        /// <summary>
        /// Width of the item. Layout space is included. Scale is not included.
        /// </summary>
        public virtual float Width 
        { 
            get { return m_width; } 

            internal set 
            {
                if (m_width != value)
                {
                    m_width = value;
                    forceRefresh();
                }
            } 
        }

        /// <summary>
        /// Height of the item. Layout space is not included. Scale is not included.
        /// </summary>
        public abstract float StaticHeight { get; }

        /// <summary>
        /// Width of the item. Layout space is not included. Scale is not included.
        /// </summary>
        public abstract float StaticWidth { get; }

        /// <summary>
        /// Represents the Vector2(StaticWidth, StaticHeight) of the object
        /// </summary>
        public virtual Vector2 StaticSize { get { return new Vector2(StaticWidth, StaticHeight); } }

        /// <summary>
        /// Measures the Width of the item. Layout space is not included. Scale is included.
        /// </summary>
        public float MeasureWidth 
        { 
            get 
            {
                if (Animation == AnimateType.Size)
                    return StaticWidth * Scale.X * 1.18f;

                return StaticWidth * Scale.X;
            }
        }

        /// <summary>
        /// Measures the Height of the item. Layout space is not included. Scale is included.
        /// </summary>
        public float MeasureHeight 
        { 
            get 
            {
                if (Animation == AnimateType.Size)
                    return StaticHeight * Scale.Y * 1.18f;

                return StaticHeight * Scale.Y;
            }
        }

        /// <summary>
        /// The matrix transform of the item.
        /// </summary>
        internal virtual Matrix ItemTransform
        {
            get
            {
                Matrix rotM, scaleM, posM, temp;

                Matrix.CreateRotationZ(Rotation, out rotM);
                Matrix.CreateScale(Scale.X, Scale.Y, 1, out scaleM);
                Matrix.CreateTranslation(Position.X, Position.Y, 0, out posM);

                Matrix.Multiply(ref scaleM, ref rotM, out temp);
                Matrix.Multiply(ref temp, ref posM, out temp);
                return temp;
            }
        }

        /// <summary>
        /// Returns the Vector2(MeasuredWidth, MeasureHeight)of the item. Layout space is not included. Scale is included.
        /// </summary>
        public virtual Vector2 Measure { get { return new Vector2(MeasureWidth, MeasureHeight); } }

        /// <summary>
        /// Returns the Vector2(Width, Height) of the item. Layout space is not included. Scale is included.
        /// </summary>
        public virtual Vector2 Size { get { return new Vector2(Width, Height); } }

        /// <summary>
        /// Visibility determines if item will be drawn.
        /// </summary>
        public virtual bool Visible { get { return m_visible && (Parent == null || Parent.Visible); } set { m_visible = value; } }

        /// <summary>
        /// Sound played when item receives focus
        /// </summary>
        public SoundEffect FocusSound { get { return m_focusSound; } set {m_focusSound = value; } }

        /// <summary>
        /// Sound played when item loses focus.
        /// </summary>
        public SoundEffect CancelSound { get { return m_cancelSound; } set {m_cancelSound = value; } }

        /// <summary>
        /// Sound played when item recieves an 'A' press.
        /// </summary>
        public SoundEffect ActivateSound { get { return m_activateSound; } set { m_activateSound = value; } }

        /// <summary>
        /// The item will listen for input on this controller
        /// </summary>
        public virtual MultiController ItemController 
        { 
            get { return m_controller; }
            set 
            {
                m_controller = value;

                foreach (var v in Children)
                    v.ItemController = value;
            }
        }

        /// <summary>
        /// Starting alpha value. Default value of 0
        /// </summary>
        public virtual float StartAlpha
        {
            get { return m_alpha.StartVal; }
            set { m_alpha.StartVal = value; }
        }

        /// <summary>
        /// Ending alpha value. Default value of 1
        /// </summary>
        public virtual float EndAlpha
        {
            get { return m_alpha.EndVal; }
            set 
            { 
                m_alpha.EndVal = value;

                if (!Fade)
                    m_alpha.Val = value;
            }
        }

        /// <summary>
        /// Tint of the item. Default value of Color.White
        /// </summary>
        public virtual Color Tint { get { return m_tint; } set { m_tint = value; } }

        /// <summary>
        /// User defined data
        /// </summary>
        public object Tag { get { return m_tag; } set { m_tag = value; } }

        /// <summary>
        /// Depth of the Item. Default is Menu.ItemDrawDepth
        /// By default 0 = front -> 1 = back
        /// </summary>
        public virtual float Depth
        {
            get { return m_depth; }
            set { m_depth = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create an Item with a parent and a controller
        /// </summary>
        public Item(Item parent, MultiController c) 
        {
            Initialize(parent);
            m_controller = c;
        }

        /// <summary>
        /// Create an Item with a parent
        /// </summary>
        public Item(Item parent) 
        {
            Initialize(parent);
        }

        private void Initialize(Item parent)
        {
            if(Menu.Font == null)
                throw new Exception("Error: Cannot construct a menu item without calling Menu.LoadContent()");

            m_ItemCollection = new ItemCollection(this);
            m_pos = Vector2.Zero;
            m_activateSound = DefaultSelectSound;
            m_cancelSound = DefaultCancelSound;
            m_focusSound = DefaultFocusSound;
            m_alpha = new Animation(0, 1, FadeTime);
            m_alpha.Val = 1;
            Parent = parent;
        }
        #endregion

        #region methods

        /// <summary>
        /// Updates the Item
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
            if(Fade)
                m_alpha.Update(gameTime);

            foreach (var child in Children)
                child.Update(gameTime);

            if (!ReadyInput)
            {
                m_framesRun++;
                return;
            }

            if (!Focus)
                return;

            if (m_controller != null)
            {
                if (m_controller.State(ButtonSet.Up, InputState))
                {
                    Up();
                }
                else if (m_controller.State(ButtonSet.Down, InputState))
                {
                    Down();
                }
                else if (m_controller.State(ButtonSet.Right, InputState))
                {
                    Right();
                }
                else if (m_controller.State(ButtonSet.Left, InputState))
                {
                    Left();
                }
                else if (m_controller.State(ButtonSet.A, InputState))
                {
                    if (A() && m_activateSound != null)
                        m_activateSound.Play(0.5f, 0f, 0f);
                }
                else if (m_controller.State(ButtonSet.Start, InputState))
                {
                    Start();
                }
                else if (m_controller.State(ButtonSet.Back, InputState))
                {
                    Back();
                }
                if(m_controller.State(ButtonSet.B, InputState))
                {
                    B();
                    if ((Focus == false || ForceCancelSound) && m_cancelSound != null)
                        m_cancelSound.Play(0.5f, 0f, 0f);
                }
                else
                {
                    return;
                }
                m_framesRun = 0;
            }
        }

        #region Delegate Extraction Layer
        internal virtual bool Up()
        {
            return Invoke(OnUp);
        }

        internal virtual bool Down()
        {
            return Invoke(OnDown);
        }

        internal virtual bool Left()
        {
            return Invoke(OnLeft);
        }

        internal virtual bool Right()
        {
            return Invoke(OnRight);
        }

        internal virtual bool A()
        {
            return Invoke(OnA);
        }

        internal virtual bool B()
        {
            return Invoke(OnB);
        }

        internal virtual bool Start()
        {
            return Invoke(OnStart);
        }

        internal virtual bool Back()
        {
            return Invoke(OnBack);
        }
        #endregion

        /// <summary>
        /// Draws the Item
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;

            Draw(gameTime, spriteBatch, Matrix.Identity);
        }

        /// <summary>
        /// Internal Draw with matrix transform. Used for parent->child scale, rotation, position transformations
        /// </summary>
        internal abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix transform);


        private void recursiveReset()
        {
            if(Fade)
                m_alpha.Reset();

            foreach (var v in Children)
                v.recursiveReset();
        }

        /// <summary>
        /// Reset the Item to a fresh state
        /// </summary>
        public virtual void Reset(bool isFocus)
        {
            recursiveReset();

            if (isFocus) //don't play initial focus sound
            {
                m_Focus = true;

                if (OnFocus != null) //intentionally not using Invoke to not lose focus of item
                    OnFocus();

                m_framesRun = 0;
            }
            else
                m_Focus = false;
        }

        /// <summary>
        /// Refreshes the items properties.
        /// This is to be called when specific child properties change
        /// that cause content alignment to change
        /// </summary>
        internal virtual void refreshItem()
        {
            foreach (var v in Children)
                v.refreshItem();

            if ((Width == 0 && Height == 0) || Parent == null)
                return;

            Vector2 pos = LayoutPosition;

            switch (HorizontalAlignment)
            {
                case HAlign.Left:
                    pos.X += (StaticWidth - Width) / 2;
                    break;
                case HAlign.Right:
                    pos.X += (Width - StaticWidth) / 2;
                    break;
                case HAlign.Stretch:
                    ScaleImageToWidth(Width);
                    break;
            }

            switch (VerticalAlignment)
            {
                case VAlign.Top:
                    pos.Y += (StaticHeight - Height) / 2;
                    break;
                case VAlign.Bottom:
                    pos.Y += (Height - StaticHeight) / 2;
                    break;
                case VAlign.Stretch:
                    ScaleImageToHeight(Height);
                    break;
            }

            Position = pos + Offset;
        }
        #endregion

        #region Helper Functions
        /// <summary>
        /// Decomposes a matrix and gets the info out of it
        /// </summary>
        internal static void DecomposeMatrix(ref Matrix matrix, out Vector2 position, out float rotation, out Vector2 scale)
        {
            Vector3 position3, scale3;
            Quaternion rotationQ;
            matrix.Decompose(out scale3, out rotationQ, out position3);
            Vector2 direction = Vector2.Transform(Vector2.UnitX, rotationQ);
            rotation = (float)Math.Atan2((double)(direction.Y), (double)(direction.X));
            position = new Vector2(position3.X, position3.Y);
            scale = new Vector2(scale3.X, scale3.Y);
        }

        /// <summary>
        /// Invokes a delegate if it is not null. Returns true if a delegate was called
        /// Sets focus to false automatically. This is the primary way to switch focus between items.
        /// If KeepFocus is true, focus will not be lost automatically. KeepFocus in this case
        /// will reset itself to false automatically for next round to lose focus
        /// </summary>
        internal virtual bool Invoke(MenuAction action)
        {
            if ( action != null )
            {
                action();

                if (KeepFocus)
                    KeepFocus = false;
                else
                    Focus = false;
            }

            return action != null;
        }

        /// <summary>
        /// Primary way of finding exactly where an object is on the screen
        /// Position is relative to parents, this way you can see the absolute position
        /// </summary>
        public virtual Vector2 screenCoords()
        {
            Vector2 pos = Position;
            Item p = Parent;
            while (p != null)
            {
                pos += p.Position;
                p = p.Parent;
            }
            return pos;
        }

        /// <summary>
        /// Combine matrix together via matrix multiply
        /// </summary>
        internal static Matrix CombineMatrix(Matrix lhs, ref Matrix rhs)
        {
            Matrix global;
            Matrix.Multiply(ref lhs, ref rhs, out global);
            return global;
        }

        /// <summary>
        /// Returns the animation value for the menu item
        /// </summary>
        protected float AnimationValue(GameTime gameTime)
        {
            double animVal = 1.0;
            if (Focus)
            {
                switch (m_animation)
                {
                    case AnimateType.Size:
                        double ms = gameTime.TotalGameTime.TotalMilliseconds;
                        double halfRemainder =  doubleMod(ms, m_animCycleTime) - (m_animCycleTime / 2);
                        double ratio = halfRemainder / (m_animCycleTime / 2);
                        animVal = 0.18 * Math.Abs(Math.Sin(ratio)) + 1;
                        break;
                }
            }
            
            return (float)animVal;
        }

        /// <summary>
        /// Scales the item to fit the desired width
        /// </summary>
        public virtual void ScaleImageToWidth(float width)
        {
            if(StaticWidth != 0)
                Scale = new Vector2(width / StaticWidth, Scale.Y);
        }

        /// <summary>
        /// Scales the item to fit the desired height
        /// </summary>
        public virtual void ScaleImageToHeight(float height)
        {
            if(StaticHeight != 0)
                Scale = new Vector2(Scale.X, height / StaticHeight);
        }

        /// <summary>
        /// Transform of button with animation transform included. (scale, grow / shrink)
        /// </summary>
        internal virtual Matrix AnimationTransform(GameTime gameTime)
        {
            Matrix animScale, local;
            local = ItemTransform;
            float fAnimVal = AnimationValue(gameTime);
            Matrix.CreateScale(fAnimVal, fAnimVal, 1, out animScale);
            return Item.CombineMatrix(animScale, ref local);
        }

        /// <summary>
        /// helper function used for animation
        /// </summary>
        internal static double doubleMod(double lhs, double rhs)
        {
            return Convert.ToDouble(Convert.ToDecimal(lhs) % Convert.ToDecimal(rhs));
        }

        internal virtual void childAdded(Item mi)
        {
            forceRefresh();
        }

        internal virtual void childRemoved(Item mi)
        {
            forceRefresh();
        }

        /// <summary>
        /// Create a texture that has a border of 'thickness' pixels. 
        /// All inside pixels are Color(0,0,0,0)
        /// </summary>
        public static Texture2D CreateRectangleBorder(int width, int height, int thickness, Color col)
        {
            return CreateBorderFilledRectangle(width, height, thickness, col, new Color(0, 0, 0, 0));
        }

        /// <summary>
        /// Create a texture that has each pixel filled with color 'col'
        /// </summary>
        public static Texture2D CreateFilledRectangle(int width, int height, Color col)
        {
            if ( width == 0 || height == 0 || col == null)
                return null;

            Color[] colors;
            Texture2D rect = new Texture2D(Menu.GraphicsDevice, width, height);
            colors = new Color[width * height];

            for (int i = 0; i < colors.Length; i++)
                colors[i] = col;

            rect.SetData(colors);

            return rect;
        }

        /// <summary>
        /// Create a texture that has a border of 'thickness' with color 'borderCol' and filled with 'FillColor'
        /// </summary>
        public static Texture2D CreateBorderFilledRectangle(int width, int height, int thickness, Color borderCol, Color FillColor)
        {
            if ( width == 0 || height == 0 || thickness < 0 || borderCol == null)
                return null;

            Color[] colors;
            Texture2D border = new Texture2D(Menu.GraphicsDevice, width, height);
            colors = new Color[width * height];

            for (int i = 0; i < colors.Length; i++)
                colors[i] = FillColor;

            //top
            for (int i = 0; i < width * thickness; i++)
                colors[i] = borderCol;

            //sides
            for (int i = 0; i < colors.Length; i += width)
            {
                for (int j = 0; j < thickness && j < width; j++) //left side
                    colors[i + j] = borderCol;
                for (int j = width - 1; j >= width - thickness && j > 0; j--) //right side
                    colors[i + j] = borderCol;
            }

            //bottom
            for (int i = width * (height - thickness); i < colors.Length; i++)
                colors[i] = borderCol;

            border.SetData(colors);
            return border;

        }
        #endregion
    }

}

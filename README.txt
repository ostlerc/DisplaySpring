DisplaySpring - XNA Menu Creator Library

  Display Spring was created by Chad Ostler.
  The purpose of Display Spring is to provide
  simple API to create flexible and complex menus.

  Please send all bug reports to ostler.c@gmail.com

Contents 
  I.   Getting started
  II.  Installation
  III. Examples
  IV.  Help 

I - Getting started
---------------------

  a. Required setup
  b. Optional Setup
  c. Creating a Menu

  a. Required Setup
  -----------------

    1. call Menu.LoadContent(GraphicsDevice, Font) (In LoadContent)
        ex. Menu.LoadContent(GraphicsDevice, MyFont);

    2. In LoadContent() Instantiate your menu
        ex. MyMenu newMenu = new MyMenu(...);

    3. In the Update() function, add yourMenu.Update()
        ex. newMenu.Update(gameTime);

    4. In the Draw() function, add yourMenu.Draw()
        ex. newMenu.Draw(gameTime, spriteBatch);

  b. Optional Setup
  -----------------

    Also in the LoadContent (wherever you want really,
    but load content is a good place to override these)

    Menu.DefaultCloseSound
    Item.DefaultCancelSound
    Item.DefaultFocusSound
    Item.DefaultSelectSound

    NOTE: a null value means no sound will play

  c. Creating a Menu
  -----------------

    1. Create a new class and inherit from DisplaySpring.Menu
    2. In your constructor create and add Items to BaseFrame
    3. Override functions such as Draw, Update, Reset for 
       more customization

II - Installation
---------------------
  1. Add DisplaySpring.dll as a reference
     -  Do this by right click on your project, and add reference

    Note: you will also want to copy the DisplaySpring.XML with your .dll	
          This is so you get the documentation with the library

  2. That's it!


III - Examples
---------------------
  For examples reference the example project 'Display Spring Demo'
  Also you can go to www.displayspring.com

IV. - Help
---------------------

  For more help, reference the API help file 
  found in the root of the project.

  DisplaySpringHelp.chm

  Or you can go online to displayspring.com 
  and see if anyone else has had the same
  problems as you.

  Last but not least, you can email me!
  ostler.c@gmail.com

  Please explain in detail your problem. It is
  always helpful to provide a code sample.

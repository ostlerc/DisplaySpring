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
  V.   License

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

  For help you can go online to displayspring.com 
  and see if anyone else has had the same
  problems as you.

  Or, you can email me!
  ostler.c@gmail.com

  Please explain in detail your problem. It is
  always helpful to provide a code sample.

IV. - License
---------------------
  DisplaySpring is released under the Ms-PL License, as outlined below.

Microsoft Public License (MS-PL)

This license governs use of the accompanying software. If you use the software, you
accept this license. If you do not accept the license, do not use the software.

1. Definitions
The terms "reproduce," "reproduction," "derivative works," and "distribution" have the
same meaning here as under U.S. copyright law.
A "contribution" is the original software, or any additions or changes to the software.
A "contributor" is any person that distributes its contribution under this license.
"Licensed patents" are a contributor's patent claims that read directly on its contribution.

2. Grant of Rights
(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.

3. Conditions and Limitations
(A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
(B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.
(C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.
(D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.
(E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.

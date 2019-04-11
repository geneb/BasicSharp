This is the language interpreter and user client for my OpenSBP project.  
The intepreter is based upon the BasicSharp project (https://github.com/Timu5/BasicSharp) 
written by Mateusz Muszyñski.  The BasicSharp project was chosen because it was a very simple,
BASIC-like interpreter that had good, very extensible, "bones".

Over time, this interpreter and interface should fully conform to the language specification at 
http://www.opensbp.com.
I'll note right now that there are some small incompatibilities with existing ShopBot Program code.
The issues are minor - things like all strings must be quoted in my engine, whereas they don't according
to the OpenSBP spec.  I may add that later, but it would require bending the lexer into shapes it was never
intended to form. :)

Updates...

[+]10Apr19[+]
-------------
You may notice that the user interface is nearly indentical to the ShopBot Control software interface.
This was done on purpose and it will likely change in the future as the software becomes more feature complete.
It will change much more quickly if SBT yells at me about it. :)

The OpenSBP Client software uses the following custom controls:

LEDBulb by Steve Marsh: (CPOL License) https://www.codeproject.com/Articles/114122/A-Simple-Vector-Based-LED-User-Control
SevenSegment by Dmitry Brant https://github.com/dbrant/SevenSegment

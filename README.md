This is the language interpreter and user client for my OpenSBP project.  
The intepreter is based upon the BasicSharp project (https://github.com/Timu5/BasicSharp) 
written by Mateusz Muszyñski.  The BasicSharp project was chosen because it was a very simple,
BASIC-like interpreter that had good, very extensible, "bones".

Over time, this interpreter will fully conform to the language specification at 
http://www.opensbp.com.

Below is Mateusz Muszyñski's orginal README.md file.

BasicSharp
====
Simple BASIC interpreter written in C#. Language syntax is modernize version of BASIC, see example below.

Example
-------
```BlitzBasic
print "Hello World"

let a = 10
print "Variable a: " + a

let b = 20
print "a+b=" + (a+b)

if a = 10 then
    print "True"
else
    print "False"
endif

for i = 1 to 10
    print i
next i

goto mylabel
print "False"

mylabel:
Print "True"

```

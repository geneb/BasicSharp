' *******************************************************
' * This program is designed to test out all the various
' * OpenSBP language features.
' *******************************************************
' * 17Mar18 gwb Started.
' *

print "Starting Tests."
gosub assignment_tests

end
assignment_tests:
'*******************
'* Assignment tests*
'*******************
print "Assignment Tests"

c = "a"
print "Assigned " + c + " to c."
c = c + " b"
print "Concat with '+' operator: c is now " + c
c = "& contat" & "enation."
print "Concat with '&' operator: " + c

&b = "A User Variable"   : REM end of line comment
print "&b is now " + &b
print "&b is now (&-concat): " & &b

&bigNum = 224.6250
print "bigNum is " + &bigNum
&bigNum = &bigNum * 2.31 - 2
print "bigNum is now " + &bigNum

return



'print &b + " is the value of &b."
'&b = &b & " adding more..."
'print &b + " is now the value of &b."
%x = "testing"
%(4) = 6  
print "The value of %(1) is " + %(1)
print "The value of %(4) is " + %(4)
print "The value of %x is " + %x

gosub mylabel
print "We returned! Yay!"

print "Hello World"

let a = 10
print "Variable a: " + a

let b = 20
print "a+b=" + (a+b)

if a = 10 then
	print "a is 10"
else
	print "a is not 10"
end if

for i = 1 to 10
	print i
next i

done:
	end
	
mylabel:
Print "subroutine mylabel hit."
return
&b = "A User Variable"  ' rem at the end of a line?

print &b + " is the value of &b."
&b = &b & " adding more..."
print &b + " is now the value of &b."

%(4) = 6  
print "The value of %(4) is " + %(4)

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
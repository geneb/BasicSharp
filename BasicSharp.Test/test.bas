gosub mylabel
print "We returned! Yay!"

let b = 5
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

goto mylabel
print "False"


mylabel:
Print "mylabel hit."
return
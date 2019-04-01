' *******************************************************
' * This program is designed to test out all the various
' * OpenSBP language features.
' *******************************************************
' * 17Mar18 gwb Started.
' *
' Strict On  ' If this is on, only & prefixed variables will work (besides system vars)

print "Starting Tests."  ' end of line remark.
' gosub assignment_tests   ' another end of line remark.
' gosub file_write_test
gosub file_read_test

end
assignment_tests:
'*******************
'* Assignment tests*
'*******************
print "Assignment Tests"

&c = "a"
print "Assigned " + &c + " to &c."
&c = &c + " b"
print "Concat with '+' operator: c is now " + &c
&c = "& contat" & "enation."
print "Concat with '&' operator: " + &c

&b = "A User Variable"   : REM end of line comment
print "&b is now " + &b
print "&b is now (&-concat): " & &b

&bigNum = 224.6250
print "bigNum is " + &bigNum
&bigNum = &bigNum * 2.31 - 2
print "bigNum is now " + &bigNum

return

file_write_test:
'**************************
'* File manipulation tests*
'**************************
print "File Create/Write Tests"

	open "test.file" for output as #1 ' append works too!
	&test = "test variable |"
	write #1, "This is some data", "and more data", &test;
	&a = "data1"
	&b = "data2"
	&c = "data3"
    write #1, " (on same line)", &a, &b, &c, &a, "testing", &c, &a
	write #1, "M2 5.00";
	write #1, ", 3.400"
	' examples from Page 25
	write #1, "The next line should read,'&myVar = 23.5'"
	&someVar = 23.5
	write #1; "&myVar = "; &someVar
	write #1, "The next line should read, 'M2, 23.5, 23.5'"

	&someVar = 23.5
	write #1; "M2, "; &someVar; ", "; &someVar
	write #1, "this is the end of the file."
	close #1
	' now we try reading it back!
	open "test.file" for input as #1
	input #1, &field1, &field2
	print "&field1 = " + &field1
	print "&field2 = " + &field2
	close #1
	return
	
file_read_test:
	' This is a more specific test of the input parsing routine.
	open "d:\\test.txt" for input as #5
	input #5,&val1
	print "&val1 = " + &val1
	input #5, &val2
	print "&val2 = " + &val2
	input #5, &val3
	print "&val3 = "+&val3
	input #5, &val4, &val5, &val6, &val7
	print "&val4 = "+&val4
	print "&val5 = "+&val5
	print "&val6 = "+&val6
	print "&val7 = "+ &val7
	input #5, &val8
	print "&val8 = "+ &val8
	close #5	
	
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
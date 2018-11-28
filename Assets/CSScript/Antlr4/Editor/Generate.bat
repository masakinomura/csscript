REM % Add Java to your PATH, if it is not already there
SET CLASSPATH=.;.\antlr-4.5-complete.jar;%CLASSPATH%
java org.antlr.v4.Tool %* -Dlanguage=CSharp -visitor -no-listener -o ..\..\Autogen CSScript.g4
title IP Rainbow Pinger - Made by RavelCros_Cro - V2
echo off & cls
color E
mode con lines=20 cols=70
echo   ____                 _  ____                   ____            
echo  !  _ \ __ ___   _____! !/ ___!_ __ ___  ___    / ___!_ __ ___   
echo  ! !_) / _` \ \ / / _ \ ! !   ! '__/ _ \/ __!  ! !   ! '__/ _ \  
echo  !  _ ! (_! !\ V /  __/ ! !___! ! ! (_) \__ \  ! !___! ! ! (_) !
echo  !_! \_\__,_! \_/ \___!_!\____!_!  \___/!___/___\____!_!  \___/ 
echo.                                                                                             
set /p IP=Enter User IP:
echo.
:rainbow
PING -n 1 %IP% | FIND "TTL="
IF ERRORLEVEL 1 (SET in=c & echo  %IP% IS OFFLINE...)
ping -t 2 0 10 127.0.0.1 >nul
color 01
PING -n 1 %IP% | FIND "TTL="
IF ERRORLEVEL 1 (SET in=c & echo  %IP% IS OFFLINE...)
ping -t 2 0 10 127.0.0.1 >nul
color 02
PING -n 1 %IP% | FIND "TTL="
IF ERRORLEVEL 1 (SET in=c & echo  %IP% IS OFFLINE...) 
ping -t 2 0 10 127.0.0.1 >nul
color 03
PING -n 1 %IP% | FIND "TTL="
IF ERRORLEVEL 1 (SET in=c & echo  %IP% IS OFFLINE...) 
ping -t 2 0 10 127.0.0.1 >nul
color 04
PING -n 1 %IP% | FIND "TTL="
IF ERRORLEVEL 1 (SET in=c & echo  %IP% IS OFFLINE...)
ping -t 2 0 10 127.0.0.1 >nul
color 05
PING -n 1 %IP% | FIND "TTL="
IF ERRORLEVEL 1 (SET in=c & echo  %IP% IS OFFLINE...) 
ping -t 2 0 10 127.0.0.1 >nul
color 06
PING -n 1 %IP% | FIND "TTL="
IF ERRORLEVEL 1 (SET in=c & echo  %IP% IS OFFLINE...) 
ping -t 2 0 10 127.0.0.1 >nul
color 07
PING -n 1 %IP% | FIND "TTL="
IF ERRORLEVEL 1 (SET in=c & echo  %IP% IS OFFLINE...) 
ping -t 2 0 10 127.0.0.1 >nul
color 08
PING -n 1 %IP% | FIND "TTL="
IF ERRORLEVEL 1 (SET in=c & echo  %IP% IS OFFLINE...) 
ping -t 2 0 10 127.0.0.1 >nul
color 09
PING -n 1 %IP% | FIND "TTL="
IF ERRORLEVEL 1 (SET in=c & echo  %IP% IS OFFLINE...)
ping -t 2 0 10 127.0.0.1 >nul
color 0b
PING -n 1 %IP% | FIND "TTL="
IF ERRORLEVEL 1 (SET in=c & echo  %IP% IS OFFLINE...) 
ping -t 2 0 10 127.0.0.1 >nul
color 0c
PING -n 1 %IP% | FIND "TTL="
IF ERRORLEVEL 1 (SET in=c & echo  %IP% IS OFFLINE...) 
ping -t 2 0 10 127.0.0.1 >nul
color 0d
PING -n 1 %IP% | FIND "TTL="
IF ERRORLEVEL 1 (SET in=c & echo  %IP% IS OFFLINE...)
ping -t 2 0 10 127.0.0.1 >nul
color 0e
PING -n 1 %IP% | FIND "TTL="
IF ERRORLEVEL 1 (SET in=c & echo  %IP% IS OFFLINE...)
ping -t 2 0 10 127.0.0.1 >nul
color 0f
PING -n 1 %IP% | FIND "TTL="
IF ERRORLEVEL 1 (SET in=c & echo  %IP% IS OFFLINE...)
ping -t 2 0 10 127.0.0.1 >nul
goto rainbow

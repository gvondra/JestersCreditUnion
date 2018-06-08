@echo off
setlocal enabledelayedexpansion

resgen Index.restext JestersCreditUnion.Core.Form.Style.Index.resources /str:vb,JestersCreditUnion.Core.Form.Style,Index,Index.vb /publicClass

set tFiles=
for /r %%i In (*.xslt) DO set tFiles=!tFiles! /embed:%%i

al.exe /out:JestersCreditUnion.Core.Form.Style.dll /v:1.0.0.0 /title:"Core From Style" /t:lib /prod:"Jesters Credit Union" /c:en-US /embed:JestersCreditUnion.Core.Form.Style.Index.resources %tFiles%

:end
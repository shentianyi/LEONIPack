netsh http add urlacl url=http://+:9000/labelService user=everyone
netsh http add urlacl url=http://+:9000/packageService user=everyone

httpcfg.exe set urlacl /u http://+:9000/labelService/ /a "O:WDG:WDD:(A;;GA;;;WD)"
httpcfg.exe set urlacl /u http://+:9000/packageService/ /a "O:WDG:WDD:(A;;GA;;;WD)"

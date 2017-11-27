# PlasticBackupCore
Simple tools to encrypt your files before backup to the cloud.

## Pokemon GYM Badges
[![sonarcloud](https://img.shields.io/badge/solarcloud-report-blue.svg)](https://sonarcloud.io/dashboard?id=com.yoniwas.PlasticBackupCore)
[![codecov](https://codecov.io/gh/yonixw/PlasticBackupCore/branch/master/graph/badge.svg)](https://codecov.io/gh/yonixw/PlasticBackupCore)
[![Build status](https://ci.appveyor.com/api/projects/status/btvh0ioi64xjyiq1/branch/master?svg=true)](https://ci.appveyor.com/project/yonixw/plasticbackupcore/branch/master)

## Project outline:
* Copy files from many places to a single place preserving as much metadata as possible.
* Use sqlite3 for all databases needs.
* Use hash to avoid duplicate files.
* Provide solution to stupid problems (Like windows max path length in CMD)
* Allow encrypting files.

## Project master plan:
Collect files from all places (phone, tablet, HDD) to one place and encrypt it before backuping to cloud (using the provider native solution).
This project only tries to organize files in one place and encrypting it.

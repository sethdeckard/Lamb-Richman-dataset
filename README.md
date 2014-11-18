[![Build Status](https://travis-ci.org/sethdeckard/Lamb-Richman-dataset.svg?branch=master)](https://travis-ci.org/sethdeckard/Lamb-Richman-dataset)
#Lamb-Richman Dataset Extension#

LR Dataset Extension is a library that generates or extends the Lamb-Richman dataset using COOP station data from the US (NOAA/NWS) and Canada (when available).  Created by Dr. Peter Lamb and Dr Michael Richman of the University of Oklahoma School of Meteorology, the climate dataset provides max/min temperature and precipitation data from 50 uniform grid points over the US and Canada, east of the Rocky Mountains. This dataset is ideal for machine learning applications related to climate science.  The library extended the existing dataset (1949-2000) from 2001 to 2010 and it can be used to automate the extension of additional years or regenerate the entire dataset again.

###Process###
The overall process consists of the following steps:

1. Import master US station list (only performed once, contains location of stations).
2. Import master Canada station list (only once, contains location of stations).
3. Import grid station info from previous export of each type (tmininfo.txt, tmaxinfo.txt, prcpinfo.txt). These were the stations actually selected as the 50 uniform grid points for each type and they will be used again except when a substitute takes place.
4. Import US COOP data from stations for a single year (TD3200 format).
5. Import Canada raw data for the same year.
6. Export data for that year.
7. Repeat steps 3-6 for additional years.

While multiple years could be processed at once in a single database, this is not recommended due to the volume of data.  Instead I use  DataExtension.ConsoleApp to automate the process so a backup database state would be saved off for each year processed, handling one year a time, this makes checking the data for errors more feasible.

###Project Structure
* DataSetExtension - The core library
* DataSetExtension.Database - Namespace that contains database management classes for each set of data (SQLite)
* DataSetExtension.Import - Namespace that contains import classes for each format
* DataSetExtension.ImportConsole - A console app that aids in import-related tasks
* DataSetExtension.ExportConsole - A console app that aids in export-related tasks
* DataSetExtension.Tests - Unit tests for the library
* DataSetExtension.ConsoleApp - App used to automate the entire process for multiple years, code MUST be modified before usage.

###Station Substitution###
Sometimes a COOP station will not have data for a particular day for a number of various reasons, or the measurement will have a flag that denotes it's likely an error.  In this case the library selects the nearest station (based on latitude and longitude) that has data for that day and uses it as a substitute station.

###Missing Canada Data###
Due to budget cuts there was missing Canada data at the time of extension, the data is said to exist on backup tape but needs to be made available online. When this data becomes available online these years can be processed again.


###Requirements and Dependencies###
The project was built in Mono on OS X, in theory it should work on Windows as well but this has not been tested yet. It uses SQLite and Dapper for data storage during processing. All the tests pass on Mono 3.10.0.

    #build project
    xbuild
    
    #run unit tests
    ./run-tests.sh


###Copyright###
Copyright (C) 2014  Seth Deckard, See LICENSE.txt for details.
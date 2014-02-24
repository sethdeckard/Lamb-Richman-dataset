#Lamb-Richman Dataset Extension#

LR Dataset Extension is a library that extends the Lamb-Richman dataset using COOP station data from the US (NOAA/NWS) and Canada (when available).  Created by Dr. Peter Lamb and Dr Michael Richman of the University of Oklahoma School of Meteorology, the climate dataset provides max/min temperature and precipitation data from 50 uniform grid points over the US and Canada, east of the Rocky Mountains. This dataset is ideal for machine learning applications related to climate science.  The library extended the existing dataset from 2000 to 2010, including filling in some gaps in the 90s. Fully automating a time consuming semi-manual process, it can be used to automate the extension of additional years.

###Station Substitution###
Sometimes a COOP station will not have data for a particular day for a number of various reasons, or the measurement will have a flag that denotes it's likely an error.  In this case the library selects the nearest station (based on latitude and longitude) that has data for that day and uses it as a substitute station.

###Missing Canada Data###
Due to budget cuts there was missing Canada data at the time of extension, the data is said to exist on backup tape but needs to be made available online.

###Process###
The overall process consists of the following steps:

1. Import master US station list (only performed once, contains location of stations).
2. Import master Canada station list (only once, contains location of stations).
3. Import grid station info from previous import of each type (tmininfo.txt, tmaxinfo.txt, prcpinfo.txt). These were the stations actually selected as the 50 uniform grid points for each type and they will be used again except when a substitute takes place.
4. Import US COOP data from stations for a single year (TD3200 format).
5. Import Canada raw data for the same year.
6. Export data for that year.

While multiple years could be processed at once this is not recommended due to the volume of data.  Instead I use  DataExtension.ConsoleApp to automate the process so a backup database state would be saved off for each year processed, this makes checking the data for errors possible.

* DataSetExtension - The library
* DataSetExtension.Database - Namespace that contains database management classes for each set of data (SQLite)
* DataSetExtension.Import - Namespace that contains import classes for each format
* DataSetExtension.ImportConsole - A console app that aids in import-related tasks
* DataSetExtension.ExportConsole - A console app that aids in export-related tasks
* DataSetExtension.Tests - Unit tests for the library
* DataSetExtension.ConsoleApp - App used to automate the entire process for multiple years, code MUST be modified before usage.

###Requirements and Dependencies###
The project was built in Mono on OS X but should work on Windows as well. It uses SQLite and Dapper for data storage during processing.


###Copyright###
Copyright (C) 2011  Seth Deckard, See LICENSE.txt for details.
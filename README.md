# A simple shell application in .NET C#

This program is a simple shell application written in .NET C#, written for the fun of it. 

## Courtesy to:

I have borrowed ideas from these two links mainly, in order to set up the disposing of child processes when the shell goes down:

* https://stackoverflow.com/questions/6266820/working-example-of-createjobobject-setinformationjobobject-pinvoke-in-net/9164742#9164742
* https://stackoverflow.com/questions/7189117/find-all-child-processes-of-my-own-net-process-find-out-if-a-given-process-is

## Main features:
* Commands are executed as new processes, use "&" to run as background process.
* The processes are grouped together as a job, to make sure that the OS cleans them up if the shell should fail somehow

## Points to improve:
* Along the way it got Windows specific, a future shell that is platform agnostic would be nice.
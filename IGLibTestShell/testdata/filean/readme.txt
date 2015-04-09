
This directory contains an example and template for using the uniform file
interface.

The direct analysis is implemented as a separate program, which exchanges
analysis parameters and response fuctions (and eventually their gradients)
with Inverse through files.

A standard format for analysis input and output file is defined (see the
Inverse manual "Solving optimization problems"). 

In the example set up in this directory, the role of the analysis program
is performed by Inverse with the "analysis.cm" command file. This can be
replaced by a small program whose source code is included in the file
"analysis.c".

FOR DEVELOPERS:
Original of "analysis.c" is contained in (...)/c/analysis.h.
Original of this directory is contained in (...)/c/filean. For users,
directory resides in $IGHOME/inverse/filean/ , but can also be downloded
separately as a zip file from the download page.



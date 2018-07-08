# openSTIGtool
This is an open source tool to read, modify, view, and report on STIGs from DISA. The checklist files are just XML files and the viewer is horrible. This started with STIG version 4.6 and the STIG Viewer 2.7.1 technology. I make no representation on earlier ones as you have to have the latest. From here on out, we will use this as the base version of the ASD STIG.

Other STIGs will be available as time permits (i.e. database server, database instance, windows server, linux, etc.)

## Functions of this tool

Some of the things this tool will (eventually) do are listed below:

* read in checklist files
* write out checklist files
* online wizard to ask questions and mark N/A for those that don't apply by default (i.e. not a web app so no cookies)
* reporting on metrics for CAT 1, 2, 3 items and percentage of completion
* simple Dockerfile or Buildah script to create a quick image to run
* Export to EXCEL with the columns of your choice for easy viewing/emailing information
* save out a default checklist file

## License
This is licensed under the Apache License.

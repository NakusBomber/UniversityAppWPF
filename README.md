# Task 8 - WPF

Create a WPF (WinForms) application for working with data from task 6:
STUDENTS (STUDENT_ID, GROUP_ID, FIRST_NAME, LAST_NAME)
GROUPS (GROUP_ID,COURSE_ID, NAME)
COURSES (COURSE_ID, NAME, DESCRIPTION)
And extend it for a new entity - Teacher.

On the default page – show a list of courses. When the course is selected - show a list of groups for the selected course. When the group is selected - show a list of students for the selected group. (You can replace course and group lists with a treeview)

A separate page for create/delete groups and edit group (change group name, select/update teacher of group). 

It’s also necessary to add functionality (buttons) for export/import a list of students of a group to a csv file (separator is “,”). Before importing students into a group, you have to clear the group where you are uploading new students.
A group can not be deleted if there is at least one student in this group.

There should be an ability to create a docx/pdf file with a list of the group, with the following content:
The document title:
- the course name
- the group name
The document itself:
- a numbered list of students (full name)

A separate page for editing students (add, update and delete), Change student data (name, surname)

Add a new entity Teacher (name, surname). Add the page for editing (add, update and delete). It is also necessary that each group of students could have a tutor.

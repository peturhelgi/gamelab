# Game Programming Lab

## Team 1 - The Great Escape

 * Einarsson, Pétur Helgi
 * Hannesson, Bjarni
 * Mesot, Nicolas
 * Radoescu, Andreea-Maria
 * Ringeisen, Simon

### Description
Very cool and fun game everyone loves to play.

## Meetings
We meet on Tuesday (after class) and on Friday (12-14). 
The agenda of the meetings will be:
1. What has everybody done?
1.1. If necessary: Discussion about design decisions
1.2. Decide if the task is ready to start mergining process
2. Looking at next task, distributing them, incl. deadlines. To manage the Tasks we use GitLabs Boards
 
For each meeting, a decision minutes is written (mettings.md)

## Coding conventions
### general procedure
For each task, we open a new branch. In this branch, we only work on this task. 

Nu Bug-fixing on the fly (i.e. outside the specific task working on)! Insteaf open a new issue, if available propose a fix.

If it is somehow possible, we stick to the given structure. If this is not possible, we first discuss this in a meeting, before starting to code.

If you want to heavily modify the code somebody else wrote, we first discuss this in a metting, before starting to code.

We do not rename classes, namespaces, public variables and functions. If the name is bad or even missleading, we add comment and create an new GitLab Issue. 

We do not write a library: We only create generic classes for things, which have more then 3 use-cases.

Write a small summary about the task implemented to the readme, to that everybody can quickly refresh their knowledge on it.


### Coding structure / style
We are not writing a documentation: comments only for code which needs explanation.

The names of classes, variables and functions should be descriptive. Styling: PublicVariables, _privateVariables, functionVariables

Linespacing:
```
if(something)
{
    something();
}
```

In general, we use the MSDN C# coding standards.

Helper classes, which serve a single purpose for a single class should be in the same file/namespace.

### Source Control
There are two control groups:
1. Andreea, Pétur, Bjarni
2. Nicolas, Simon

The workflow is the following:
1. task is assigned in meeting
2. assignee creates a new branch from the current master
3. assignee implements the task
4. in the next meeting, the assignee shows his work. The team decides if the task is completed and if the mergin process can start.
5. The assignee merges master into the task branch and fixes potential conflicts.
6. The assignee creates a merge request in GitLab and assignes someone from the other control group, how then does a code review
7. If the reviewer agrees, the request is granted and the code is merged into master.
 
**This guarantees, that there is only compiling and running code in the master branch**


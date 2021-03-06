# Context 

Our platform, **Solution1**, comes as the 1st solution for the issues our college is facing. 

It offers a series of functionalities that will positively impact the inner workings of our college, helping both teachers and students by providing a single, simple to use solution through which courses can be accessed and assignments distributed and centralized.

# Access to the Platform

In order to keep things simple, access to the platform does not require registration, access being (should be) granted through your college login information.

> TBD

# Functionalities
The core idea around which everything else works is the **Course**. Teachers can create courses and students can access them.

## Teacher's 
Each teacher can add the courses he teaches and for each one of those courses he can create (weekly) **modules**.

Each **module** can contain, depending on the teachers choice:
* a studying part 
* an assignment (homework)
* evaluation examples (tests)
* other materials beyond the course for interested students 

Beside creating modules, teachers can **grade** and also **review** the submitted solutions.

## Student's
Each student can access the courses and filter them by year or some other criterion. For each assignemnt, the student can submit his/her solution.


## Assignment upload
As *single, simple solution* is the central motto of our project, we have made it so that not only are all courses readily available, but assignment distribution and centralization is also handled by the same platform.

In order to uphold academic standards, assignment centralization will also check for plagiarism.

##Plagiarism check
Our team is comprised of young idealistic minds, striving for perfection every day. We are firm believers that no matter the circumstances, one should never cheat, after all perfection comes from within, not from a google search. Hence, we shall use MOSS (Measure of Software Similarity). MOSS is tool for automatically
detecting plagiarism. Although the algorithm behind MOSS is very proficient (more details here http://theory.stanford.edu/~aiken/publications/papers/sigmod03.pdf), it is still
up to the examiner to go and check the code suspected of foul play.
MOSS is an internet service. Our app will send source files to the central server. Once the payload is delivered, the central server returns the results in form of a URL string (e.g. http://moss.stanford.edu/results/XXXXXXXX).

For more details about MOSS, check https://theory.stanford.edu/~aiken/moss/ 

# Donate
Because we're awesome, we give you the opportunity to give us money!

Feature: SignUp
	In order to get tutoring help
	As a student disatisfied with the tutoring center
	I want to signup at peertutor.us so I can get tutored


Scenario: Sign up successful
	Given I am on the register page
	And I enter a first name of Salome
	And I enter a last name of Sanders
	And I select the major Computer Information Technology
	And I select the year 2019
	And I enter a phone 3153275398
	And I enter an email ssanders@mnsu.edu
	And I enter a password AStr0ngPassword!
	And I confirm the password entered above AStr0ngPassword!
	When I press register
	Then I should see the CourseMenu page

Feature: Render

Render a model using a template

@generate
Scenario: Generate from valid template and valid model
	Given valid template
	And valid model
	When Generate is called using template and model 
	Then the result should be correct
	
@save-template
Scenario: Save template
	Given key
	And valid model
	When Save is called using key and model
	Then model should be saved
	
@generate
Scenario: Generate from valid saved template and valid model
	Given valid saved template
	And key
	And valid model
	When Generate is called using key and model 
	Then the result should be correct
	
@generate
Scenario: Generate from valid saved template and unmatched model
	Given valid saved template
	And key
	And unmatched model
	When Generate is called using key and model 
	Then the result should be correct

@generate
Scenario: Generate from valid saved template and empty model
	Given valid saved template
	And key
	And empty model
	When Generate is called using key and model 
	Then the result should be empty string

@generate
Scenario: Generate from invalid saved template and valid model
	Given invalid saved template
	And key
	And valid model
	When Generate is called using key and model 
	Then the result should be empty string
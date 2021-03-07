package com.adf.photoapp.api.users.ui.model;

import javax.validation.constraints.Email;
import javax.validation.constraints.NotNull;
import javax.validation.constraints.Size;

public class CreateUserRequestModel {

	@NotNull(message="First name can not be null")
	@Size(min=2, max=8, message="First name must not be less than 2 charachters")
	private String firstName;
	
	@NotNull(message="Last name can not be null")
	@Size(min=2, max=8, message="Last name must not be less than 2 charachters")
	private String lastName;

	@NotNull(message="Email can not be null")
	@Email(message="Invalid email format")
	private String email;
	
	@NotNull(message="Password can not be null")
	@Size(min=8, max=16, message="Password must not be less than 8 charachters")
	private String password;
	public String getFirstName() {
		return firstName;
	}
	public void setFirstName(String firstName) {
		this.firstName = firstName;
	}
	public String getLastName() {
		return lastName;
	}
	public void setLastName(String lastName) {
		this.lastName = lastName;
	}
	public String getEmail() {
		return email;
	}
	public void setEmail(String email) {
		this.email = email;
	}
	public String getPassword() {
		return password;
	}
	public void setPassword(String password) {
		this.password = password;
	}
	
}

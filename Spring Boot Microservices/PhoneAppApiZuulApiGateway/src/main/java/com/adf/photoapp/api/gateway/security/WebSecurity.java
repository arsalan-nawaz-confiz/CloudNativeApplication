package com.adf.photoapp.api.gateway.security;

import javax.ws.rs.HttpMethod;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.env.Environment;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.config.annotation.web.configuration.WebSecurityConfigurerAdapter;
import org.springframework.security.config.http.SessionCreationPolicy;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;

@Configuration
@EnableWebSecurity
public class WebSecurity extends WebSecurityConfigurerAdapter {
	private Environment env;
	@Autowired
	public WebSecurity(Environment env)
	{
		this.env = env;
	}
	@Override
	protected void configure(HttpSecurity http) throws Exception {
		http.csrf().disable();
		http.headers().frameOptions().disable();
		http.authorizeRequests().antMatchers(HttpMethod.POST, this.env.getProperty("api.login.url.path")).permitAll()
		.antMatchers(HttpMethod.POST, this.env.getProperty("api.registeration.url.path")).permitAll()
		.antMatchers(this.env.getProperty("api.zuul.actuator.url.path")).permitAll()
		.antMatchers(this.env.getProperty("api.users.actuator.url.path")).permitAll()
		.antMatchers(this.env.getProperty("api.h2console.url.path"))
		.permitAll()
		.anyRequest().authenticated()
		.and().addFilter(new AuthorizationFilter(authenticationManager(), env));
		http.sessionManagement().sessionCreationPolicy(SessionCreationPolicy.STATELESS);
	}
}

package com.adf.photoapp.api.users.security;

import java.io.IOException;
import java.sql.Date;
import java.util.ArrayList;

import javax.servlet.FilterChain;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.springframework.core.env.Environment;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.AuthenticationException;
import org.springframework.security.core.userdetails.User;
import org.springframework.security.web.authentication.UsernamePasswordAuthenticationFilter;

import com.adf.photoapp.api.users.service.UsersService;
import com.adf.photoapp.api.users.shared.UserDto;
import com.adf.photoapp.api.users.ui.model.LoginRequestModel;
import com.fasterxml.jackson.databind.ObjectMapper;

import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.SignatureAlgorithm;

public class AuthenticationFilter extends UsernamePasswordAuthenticationFilter {
	private UsersService usersService;
	private Environment env;
	
	public AuthenticationFilter(UsersService usersService, Environment env, AuthenticationManager authenticationManager)
	{
		this.usersService = usersService;
		this.env = env;
		super.setAuthenticationManager(authenticationManager);
	}
	
	@Override
	public Authentication attemptAuthentication(HttpServletRequest req, HttpServletResponse res)
			throws AuthenticationException {
		try {
			LoginRequestModel creds = new ObjectMapper().readValue(req.getInputStream(), LoginRequestModel.class);
			return getAuthenticationManager().authenticate(
					new UsernamePasswordAuthenticationToken(creds.getEmail(), creds.getPassword(), new ArrayList<>()));
		} catch (IOException ex) {
			throw new RuntimeException();
		}
	}

	@Override
	protected void successfulAuthentication(HttpServletRequest req, HttpServletResponse res, FilterChain chain,
			Authentication auth) throws IOException, ServletException{
		String userName = ((User) auth.getPrincipal()).getUsername();
		UserDto userDetails = this.usersService.getUserDetailsByEmail(userName);
		String token = Jwts.builder().setSubject(userDetails.getUserId()).setExpiration(
				new Date(System.currentTimeMillis() + Long.parseLong(env.getProperty("token.expiration_time")))).signWith
				(SignatureAlgorithm.HS512, env.getProperty("token.secret")).compact();
		res.addHeader("Token", token);
		res.addHeader("UserId", userDetails.getUserId());
	}
}

package org.nure.gotrip.controller;

import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mockito;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.repository.RegisteredUserRepository;
import org.nure.gotrip.util.contstant.UserConstants;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.test.context.junit4.SpringRunner;
import org.springframework.test.web.servlet.MockMvc;

import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.post;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@RunWith(SpringRunner.class)
@SpringBootTest(webEnvironment = SpringBootTest.WebEnvironment.RANDOM_PORT)
@AutoConfigureMockMvc
public class RegistrationControllerTest {

	private static final String REGISTER = "/register";

	@MockBean
	RegisteredUserRepository registeredUserRepository;

	@Autowired
	private MockMvc mvc;

	@Test
	public void shouldGetPositiveResponse() throws Exception {
		String login = "trololoshka";
		String password = "1234567891011";
		String email = "trololo@gmail.com";

		mvc.perform(post(REGISTER)
				.param(UserConstants.LOGIN, login)
				.param(UserConstants.PASSWORD, password)
				.param(UserConstants.EMAIL, email))
				.andExpect(status().isOk());
	}

	@Test
	public void shouldGetNegativeResponseWhenInvalidData() throws Exception {
		String login = "trololo";
		String password = "12345";
		String email = "trololo";

		mvc.perform(post(REGISTER)
				.param(UserConstants.LOGIN, login)
				.param(UserConstants.PASSWORD, password)
				.param(UserConstants.EMAIL, email))
				.andExpect(status().isBadRequest());
	}

	@Test
	public void shouldGetNegativeResponseWhenNotUniqueLogin() throws Exception {
		String login = "trololoshka";
		String password = "1234567891011";
		String email = "trololo@gmail.com";

		Mockito.when(registeredUserRepository.save(Mockito.any(RegisteredUser.class)))
				.thenThrow(new DataIntegrityViolationException("The database contains a user with this login"));

		mvc.perform(post(REGISTER)
				.param(UserConstants.LOGIN, login)
				.param(UserConstants.PASSWORD, password)
				.param(UserConstants.EMAIL, email))
				.andExpect(status().isConflict());
	}

}
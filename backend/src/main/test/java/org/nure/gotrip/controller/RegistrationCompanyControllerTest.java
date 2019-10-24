package org.nure.gotrip.controller;

import com.jayway.jsonpath.spi.json.GsonJsonProvider;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mockito;
import org.nure.gotrip.dto.CompanyDto;
import org.nure.gotrip.model.Company;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.repository.CompanyRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.http.MediaType;
import org.springframework.test.context.junit4.SpringRunner;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders;

import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@RunWith(SpringRunner.class)
@SpringBootTest(webEnvironment = SpringBootTest.WebEnvironment.RANDOM_PORT)
@AutoConfigureMockMvc
public class RegistrationCompanyControllerTest {

	private static final String COMPANY_REGISTRATION = "/company/registration";

	@MockBean
	CompanyRepository companyRepository;

	@Autowired
	private MockMvc mvc;

	@Test
	public void shouldGetPositiveResponse() throws Exception {
		CompanyDto companyDto = new CompanyDto("name", "fiasko@gmail.com", new RegisteredUser(), "lol");
		GsonJsonProvider gsonJsonProvider = new GsonJsonProvider();

		mvc.perform(MockMvcRequestBuilders.post(COMPANY_REGISTRATION)
				.content(gsonJsonProvider.toJson(companyDto))
				.contentType(MediaType.APPLICATION_JSON))
				.andExpect(status().isOk())
				.andReturn();
	}

	@Test
	public void shouldGetBadRequestResponse() throws Exception {
		CompanyDto companyDto = new CompanyDto("name", "fiasko", new RegisteredUser(), "lol");
		GsonJsonProvider gsonJsonProvider = new GsonJsonProvider();

		mvc.perform(MockMvcRequestBuilders.post(COMPANY_REGISTRATION)
				.content(gsonJsonProvider.toJson(companyDto))
				.contentType(MediaType.APPLICATION_JSON))
				.andExpect(status().isBadRequest())
				.andReturn();
	}

	@Test
	public void shouldGetConflictResponse() throws Exception {
		CompanyDto companyDto = new CompanyDto("name", "fiasko@gmail.com", new RegisteredUser(), "lol");
		GsonJsonProvider gsonJsonProvider = new GsonJsonProvider();

		Mockito.when(companyRepository.save(Mockito.any(Company.class)))
				.thenThrow(new DataIntegrityViolationException("The database contains a company with this name"));

		mvc.perform(MockMvcRequestBuilders.post(COMPANY_REGISTRATION)
				.content(gsonJsonProvider.toJson(companyDto))
				.contentType(MediaType.APPLICATION_JSON))
				.andExpect(status().isConflict())
				.andReturn();
	}

}
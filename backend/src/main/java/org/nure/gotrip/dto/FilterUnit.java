package org.nure.gotrip.dto;

import lombok.Getter;
import lombok.Setter;

import java.util.Map;

@Getter
@Setter
public class FilterUnit {

	private Map<String, Filter> filters;
	private Map<String, String> search;
    private Map<String, String> semiFilters;
	private String sortingCriterion;

	@Getter
	@Setter
	public static class Filter {
		String from;
		String to;
	}

	public Filter getFilter(String filterName) {
		return filters.get(filterName);
	}
}

package org.nure.gotrip.dto;

import lombok.Getter;
import lombok.Setter;

import java.util.List;

@Getter
@Setter
public class FilterUnit {

    private List<Filter> filters;
    private List<String> sortingCriteria;
    private String tourSubstring;
    private String locationSubstring;

    @Getter
    @Setter
    public static class Filter{

        String name;
        String from;
        String to;

        public Filter(){

        }

        public Filter(String name){
            this.name = name;
        }

        @Override
        public boolean equals(Object o) {
            if(o instanceof Filter){
                Filter filter = (Filter)o;
                return name.equals(filter.name);
            }else{
                return false;
            }
        }
    }

    public Filter getFilterByName(String filterName){
        return filters.stream().filter(filter -> filter.name.equals(filterName)).findFirst().get();
    }
}

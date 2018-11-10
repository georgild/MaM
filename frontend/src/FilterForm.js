import React from 'react';
import {PropTypes} from 'prop-types';

let FilterForm = React.createClass({

    getInitialState : function() {
        return { ArrivesAt: '', ArrivesFrom: '', CompanyName: '', Currency: 'USD' };
    },

    propTypes : {
        onFilterSubmit: PropTypes.func
    },

    handleSubmit : function(e) {

        e.preventDefault();

        var ArrivesAt = this.state.ArrivesAt;
        var ArrivesFrom = this.state.ArrivesFrom.trim();
        var CompanyName = this.state.CompanyName.trim();
        var Currency = this.state.Currency.trim();

        var filters = [];

        if (ArrivesAt) {
            filters.push({
                Property: 'FinalStop.ArrivalDate',
                Value: (new Date(ArrivesAt)).getTime(), 
                Operator: 'eq'
            })
        }

        if (ArrivesFrom) {
            filters.push({
                Property: 'InitialStop.City',
                Value: ArrivesFrom, 
                Operator: 'eq'
            })
        }

        if (CompanyName) {
            filters.push({
                Property: 'CompanyName',
                Value: CompanyName, 
                Operator: 'eq'
            })
        }

        this.props.onFilterSubmit(filters, Currency);
    },

    handleArrivesAtChange : function(e) {
        this.setState({ ArrivesAt: e.target.value });
    },

    handleArrivesFromChange : function(e) {
        this.setState({ ArrivesFrom: e.target.value });
    },

    handleCompanyNameChange : function(e) {
        this.setState({ CompanyName: e.target.value });
    },

    handleCurrencyChange : function(e) {
        this.setState({ Currency: e.target.value });
    },

    render : function() {
        return (
            <span>
                <form className="filterForm" onSubmit={this.handleSubmit}>
                    <label htmlFor="Search">Search:</label>
                    <input
                        id="Search"
                        type="text"
                        value={this.state.ArrivesFrom}
                        onChange={this.handleArrivesFromChange}
                    />
                    <input className="button" type="submit" /*disabled={!this.state.ArrivesAt && !this.state.ArrivesFrom && !this.state.CompanyName}*/ value="Search" />
                </form>
            </span>
        );
    }
});

export default FilterForm;
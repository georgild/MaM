import React from 'react';
import $ from 'jquery';
import {PropTypes} from 'prop-types';

import ReactDataGrid from 'react-data-grid';
import Toolbar from './Toolbar';

class Companies extends React.Component {

    constructor(props) {
        super(props);
        this.state = { 
            data: [], 
            filters : [],
            selectedIds: []
        };
    }

    static propTypes = {
        url: PropTypes.string,
        //initialFilters: PropTypes.JSON,
        pollInterval: PropTypes.number
    }

    columns = [{ 
        key: 'Name', 
        name: 'Name' 
    }, { 
        key: 'City', 
        name: 'City' 
    }, { 
        key: 'Email', 
        name: 'Email' 
    }]

    loadCompanies(filters) {

        $.ajax({
            url: this.props.url,
            data: 'filters=' + JSON.stringify(this.props.initialFilters.concat(filters)),
            dataType: 'json',
            async: true,
            cache: false,
            success: function (data) {
                var parsedData = [];

                if (data) {
                    data.forEach(function(company) {
                        parsedData.push({
                            Id: company.Id,
                            Name: company.Name,
                            City: company.Offices[0].City,
                            Email: company.Offices[0].Email
                        });
                    });
                }
                
                this.setState({ data: parsedData });
            }.bind(this),
            error: function (xhr, status, err) {
                console.error(this.props.url, status, err.toString());
            }.bind(this)
        });
    }

    handleFiltersSubmit = (filters, currency) => {
        this.setState({ filters: filters });
        this.setState({ currency: currency });
        this.loadArrivals(filters, currency);
    }

    onRowsSelected = (rows) => {
        //debugger;
        this.setState({selectedIds: this.state.selectedIds.concat(rows.map(r => r.row.Id))});
    }

    onRowsDeselected = (rows) => {
        let rowIds = rows.map(r => r.row.Id);
        this.setState({selectedIds: this.state.selectedIds.filter(i => rowIds.indexOf(i) === -1 )});
    }

    componentDidMount = () => {
        this.loadCompanies(this.state.filters);
        //setInterval(this.loadArrivals, this.props.pollInterval);
    }

    render() {
        return (
            <div>
                <h2>Companies Administration</h2>
                <Toolbar/>
                <ReactDataGrid
                    //ref={ node => this.grid = node }
                    enableRowSelect={true}
                    rowKey="Id"
                    rowSelection={{
                        showCheckbox: true,
                        enableShiftSelect: true,
                        onRowsSelected: this.onRowsSelected,
                        onRowsDeselected: this.onRowsDeselected,
                        selectBy: {
                            keys: {rowKey: 'Id', values: this.state.selectedIds}
                        }
                    }}
                    columns={this.columns}
                    rowGetter={rowNumber => this.state.data[rowNumber]}
                    rowsCount={this.state.data.length}
                    minHeight={500}
                />
            </div>
        );
    }
}

export default Companies;
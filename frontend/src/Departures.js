import React from 'react';
import $ from 'jquery';
import {PropTypes} from 'prop-types';

import OrderDialog from './Orders/OrderDialog';

class Departures extends React.Component {

    constructor(props) {
        super(props);
        this.state = { 
            data: [], // for test
            filters : [],
            currency: 'USD',
            dialogIsOpen: false,
            selectedRowId: ''
        };
    }

    static propTypes = {
        url: PropTypes.string,
        pollInterval: PropTypes.number
    }

    handleBuyClick = (Id) => {

        this.setState({ dialogIsOpen: true });
        this.setState({ selectedRowId: Id });
    }

    handleModalCloseClick = () => {
        this.setState({dialogIsOpen: false});
    }

    handleOrderSubmit = (email, seats) => {
        this.setState({dialogIsOpen: false});

        var postData = {
            RouteID: this.state.selectedRowId,
            User: email,
            Seats: seats
        }
        $.ajax({
            type: 'POST',
            url: '/api/v1/orders',
            data: JSON.stringify(postData),
            dataType: 'json',
            cache: false,
            success: function (data) {
                alert('Success');
            },
            error: function (xhr, status, err) {
                console.error(this.props.url, status, err.toString());
            }.bind(this)
          });
    }

    loadArrivals(filters, currency) {
        var self = this;
        $.ajax({
            url: this.props.url,
            data: 'filters=' + JSON.stringify(this.props.initialFilters.concat(filters)) + '&currency=' + currency,
            dataType: 'json',
            cache: false,
            success: function (data) {
                var parsedData = [];

                if (data) {
                    data.forEach(function(route, index) {
                        parsedData.push({
                            Id: route.Id,
                            DepartsAt: (new Date(route.InitialStop.DepartureDate)).toLocaleString(),
                            TravelsTo: route.FinalStop.City,
                            CompanyName: route.CompanyName,
                            TicketPrice: route.TicketPrice + ' ' + self.state.currency
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

    componentDidMount = () => {
        this.loadArrivals(this.state.filters, this.state.currency);
        //setInterval(this.loadArrivals, this.props.pollInterval);
    }

    render() {
        return (
            <div>
                <h2>Departures</h2>
                <table className="routes-table">
                    <tbody>
                    <tr>
                        <th>Departs At</th>
                        <th>Travels To</th>
                        <th>Company Name</th>
                        <th>Ticket Price</th>
                        <th></th>
                    </tr>
                        {Array.apply(null, this.state.data).map(function(item, i){                                        
                            return (
                                <tr key={i} >
                                    <td>{item.DepartsAt}</td>
                                    <td>{item.TravelsTo}</td>
                                    <td>{item.CompanyName}</td>
                                    <td>{item.TicketPrice}</td>
                                    <td>
                                        <button className="button grid-button" onClick={() => this.handleBuyClick(item.Id)}>Order</button>
                                    </td>
                                </tr>
                            );
                        }, this)} 
                    </tbody>
                </table> 
                <OrderDialog 
                    selectedRowId={this.state.selectedRowId}
                    dialogIsOpen={this.state.dialogIsOpen} 
                    onCloseClick={this.handleModalCloseClick}
                    onSubmitOrder={this.handleOrderSubmit}
                    rowsCount={10}
                    colsCount={4}
                />
            </div>
        );
    }
}

export default Departures;
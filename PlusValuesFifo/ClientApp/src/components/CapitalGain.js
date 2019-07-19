import ReactDOM from 'react-dom';
import React, { Component } from 'react';

export class CapitalGain extends Component {
    static displayName = CapitalGain.name;

    constructor(props) {
        super(props);
        this.capitalGain = React.createRef();
        this.handleSubmit = this.handleSubmit.bind(this);
    }


    handleSubmit(event) {
        event.preventDefault();
        const fileData = this.capitalGain.current.files[0];

        fetch('/api/plusvalues', {
            method: 'POST',
            body: fileData
        });
    }

    render() {
        return (
            <div>
                <h1>Welcome</h1>

                <p>In order to compute your capital gains, please import a CSV file containing your buy & sell events:</p>

                <form method="post" encType="multipart/form-data" onSubmit={this.handleSubmit}>
                    <input type="file" name="file" ref={this.capitalGain} text="Your input file" />
                    <button type="submit">Compute Capital Gain</button>
                </form>
            </div>
        );
    }
}

//ReactDOM.render(
//    <CapitalGain />,
//    document.getElementById('root')
//);


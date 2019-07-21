import React, { Component } from 'react';

export class CapitalGain extends Component {
    static displayName = CapitalGain.name;

    constructor(props) {
        super(props);
        //this.capitalGain = React.createRef();
        //this.handleSubmit = this.handleSubmit.bind(this);
    }

    uploadFile(file) {
        fetch('/api/plusvalues', {
            method: 'POST',
            body: file
        })
        //.then(response => response.json())
        .then(success => {
            console.log('success');
        })
        .catch(error => console.log(error));
    }


    //handleSubmit(event) {
    //    event.preventDefault();
    //    const fileData = this.capitalGain.current.files[0];
    //}

    render() {
        return (
            <div>
                <h1>Welcome</h1>

                <p>In order to compute your capital gains, please import a CSV file containing your buy & sell events:</p>

                <form method="post" encType="multipart/form-data" onSubmit={this.uploadFile}>
                    <input type="file" name="file" text="Your input file" />
                    <button type="submit">Compute Capital Gain</button>
                </form>
            </div>
        );
    }
}
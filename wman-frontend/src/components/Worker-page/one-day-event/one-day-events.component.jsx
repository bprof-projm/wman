import react, { useState } from "react";
import OneLabel from "../one-label.component.jsx/one-label.component";
import "./one-day-events-styles.css"
const OneDayEvents = (event) => {

    const [show, setShow] = useState(false);
    const toggleShow = () => {
        setShow(!show);
    }
    console.log();

    return (
        <div className='one-event' onClick={toggleShow}>
            <div className='one-event-tables'>
                <table>
                    <tr>
                        <th colSpan='2' ><b>Description:</b></th>
                    </tr>
                    <tr>
                        <th colSpan='2' >{event.event.jobDescription}</th>
                    </tr>
                    <tr>
                        <th>Start:</th>
                        <th>{event.event.estimatedStartDate}</th>
                    </tr>
                    <tr>
                        <th>Finish:</th>
                        <th>{event.event.estimatedFinishDate}</th>
                    </tr>
                </table>

                {show ?
                    <table>

                        <tr>
                            <th colSpan='2'><b>Address:</b></th>
                        </tr>
                        <tr>
                            <th>City:</th>
                            <th>{event.event.address.city}</th>
                        </tr>
                        <tr>
                            <th>Street:</th>
                            <th>{event.event.address.street}</th>
                        </tr>
                        <tr>
                            <th>ZipCode:</th>
                            <th>{event.event.address.zipCode}</th>
                        </tr>
                        <tr>
                            <th>Building number:</th>
                            <th>{event.event.address.buildingNumber}
                            </th>
                        </tr>
                        <tr>
                            {(event.event.address.floorDoor === null) ? null : <th>Floor door:</th>}
                            {(event.event.address.floorDoor === null) ? null : <th>{event.event.address.floorDoor}</th>}                           
                            
                        </tr>
                        <tr>
                            {(event.event.labels.length === 0) ? null : <th colSpan='2'><b>Labels:</b></th>}
                            
                        </tr>                       

                    </table>
                    : null}
                {show ?
                    <div className="label-container">{event.event.labels.map(label => <OneLabel
                        key={label.id}
                        name={label.content}
                        backgroundColor={label.backgroundColor}
                        textColor={label.textColor}
                    />)}</div>
                    : null}
            </div>
        </div>
    )
}
export default OneDayEvents;
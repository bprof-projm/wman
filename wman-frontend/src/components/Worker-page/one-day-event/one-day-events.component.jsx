import react, { useState } from "react";
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
                            <th>Floor door:</th>
                            <th>{event.event.address.floorDoor}</th>
                        </tr>
                        <tr>
                            <th colSpan='2'><b>Labels:</b></th>
                        </tr>
                        <tr>
                            <th colSpan='2'><b>Ide kéne majd a label card komponensnek megadni a labaleket ha be lesz mergelve</b></th>
                        </tr>

                    </table>
                    : null}
            </div>
        </div>
    )
}
export default OneDayEvents;
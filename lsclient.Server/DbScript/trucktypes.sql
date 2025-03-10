--Inserting TruckTypes

-- Insert Truck Types with SAMPLE_IMAGE_URL values into TruckType table
INSERT INTO TruckType (TRUCK_TYPE_ID, TYPE_NAME, DESCRIPTION, SAMPLE_IMAGE_URL)
VALUES
    ('TT001', 'Big Truck', 'Large truck for heavy cargo, ideal for long-haul and high-capacity deliveries.', 'Big truck'),
    ('TT002', 'Container', 'Container transport truck, suitable for large container loads.', 'Container.jpeg'),
    ('TT003', 'Flatbed Truck', 'Flatbed truck for oversized or irregularly shaped loads.', 'flatbed_truck.jpeg'),
    ('TT004', 'Motorcycle', 'Suitable for small cargo, ideal for quick deliveries within city limits.', 'motorcycle.jpeg'),
    ('TT005', 'Pickup Truck', 'Small truck for medium cargo, suitable for short-distance transport.', 'pickup.jpeg'),
    ('TT006', 'Refrigerated Truck', 'Truck with temperature control, ideal for perishable goods.', 'RefrigeratedTruck.jpeg'),
    ('TT007', 'Small Truck', 'Compact truck for medium to large deliveries within a city.', 'small_truck.jpeg'),
    ('TT008', 'Tanker Truck', 'Truck used to transport liquids or gases.', 'TankerTruck.jpeg'),
    ('TT009', 'Tow Truck', 'Vehicle equipped to tow or transport other vehicles.', 'TowTruck.jpg'),
    ('TT010', 'Van', 'Standard van for moderate cargo, ideal for light deliveries.', 'Van.jpeg'),
    ('TT011', 'Flatbed Truck', 'Flatbed truck for oversized or irregularly shaped loads.', 'Flatbed_truck.jpeg'),
    ('TT012', 'Big Truck', 'Large truck for heavy cargo, ideal for long-haul and high-capacity deliveries.', 'BigTruck.jpeg'),
    ('TT013', 'Container Truck', 'Truck designed specifically for transporting containers.', 'container.jpeg');






update TruckTypes set SAMPLE_IMAGE_URL = 'BigTruck.jpeg' WHERE TYPE_NAME = 'Big truck';
update TruckTypes set SAMPLE_IMAGE_URL = 'Container.jpeg' WHERE TYPE_NAME = 'container';
update TruckTypes set SAMPLE_IMAGE_URL = 'flatbed_truck.jpeg' WHERE TYPE_NAME = 'Flatbed truck';
update TruckTypes set SAMPLE_IMAGE_URL = 'Bajaj.jpeg' WHERE TYPE_NAME = 'motorcicle';
update TruckTypes set SAMPLE_IMAGE_URL = 'pickup.jpeg' WHERE TYPE_NAME = 'Pickup truck';
update TruckTypes set SAMPLE_IMAGE_URL = 'RefrigeratedTruck.jpeg' WHERE TYPE_NAME = 'Refrigerated Truck';
update TruckTypes set SAMPLE_IMAGE_URL = 'small_truck.jpeg' WHERE TYPE_NAME = 'Small Truck';
update TruckTypes set SAMPLE_IMAGE_URL = 'TankerTruck' WHERE TYPE_NAME = 'Tanker Truck';
update TruckTypes set SAMPLE_IMAGE_URL = 'TowTruck.jpg' WHERE TYPE_NAME = 'Tow Truck';
update TruckTypes set SAMPLE_IMAGE_URL = 'Van.jpeg' WHERE TYPE_NAME = 'Van';
update TruckTypes set SAMPLE_IMAGE_URL = 'Flatbed_truck.jpeg' WHERE TYPE_NAME = 'Flatbed Truck';
update TruckTypes set SAMPLE_IMAGE_URL = 'motorcycle.jpeg' WHERE TYPE_NAME = 'Motorcycle';
update TruckTypes set SAMPLE_IMAGE_URL = 'BigTruck.jpeg' WHERE TYPE_NAME = 'Big Truck';
update TruckTypes set SAMPLE_IMAGE_URL = 'container.jpeg' WHERE TYPE_NAME = 'Container Truck';




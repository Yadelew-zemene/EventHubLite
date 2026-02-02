sequenceDiagram
    participant User
    participant App
    participant Backend
    participant BridgeKYC as Bridge KYC UI
    participant BridgeWallet as Bridge Wallet API
    participant DB

    User ->> App: Tap "Verify Identity"
    App ->> Backend: Request KYC session
    Backend ->> BridgeKYC: Create KYC session via API
    BridgeKYC -->> Backend: Return KYC redirect URL
    Backend -->> App: Send KYC redirect URL
    App ->> User: Redirect to KYC link
    User ->> BridgeKYC: Submit documents

    alt KYC Approved
        BridgeKYC ->> Backend: kyc.approved (Webhook)
        Backend ->> DB: Update KYC status = "Approved"
        Backend ->> BridgeWallet: Create custodial wallet
        BridgeWallet -->> Backend: Wallet created
        Backend -->> App: Notify user success
    else KYC Rejected
        BridgeKYC ->> Backend: kyc.rejected (Webhook)
        Backend ->> DB: Update KYC status = "Rejected"
        Backend -->> App: Notify user failure
    end
